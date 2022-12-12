using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RCR.Tiled;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using WaterNavTiled.Data;
using WaterNavTiled.Editor.Jobs;
using Object = UnityEngine.Object;

namespace WaterNavTiled.Editor
{
#if UNITY_EDITOR
    
    [CustomEditor(typeof(Map))]
    public class MapEditor : UnityEditor.Editor
    {
        private Map map {get => target as Map;}

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            
            if(GUILayout.Button("GENERATE"))
                SetUpMap();

            EditorGUILayout.Space(25f);
            if (GUILayout.Button("TILESETS 2 UNITY"))
                Tilesets2Unity(serializedObject.FindProperty("Active_FolderPaths"));
                
            EditorGUILayout.Space(25f);
            
            //Map Settings
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MapSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TileWidth"));
            
            EditorGUILayout.Space(25f);
            
            EditorList.Show(serializedObject.FindProperty("TileSets"), EditorListOption.Buttons);
            
            EditorGUILayout.Space(25f);
            
            EditorList.Show(serializedObject.FindProperty("Active_FolderPaths"));



            if (EditorGUI.EndChangeCheck())
            {
                Validate(serializedObject.FindProperty("TileSets"));
                EditorUtility.SetDirty(map);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void SetUpMap()
        {
            
        }

        private void Tilesets2Unity(SerializedProperty list)
        {
            //ForEach localTileSet
            //Get That TileSetsImage
            //Make sure its READ/WRITE
            //Make Sure the TILESET is MULTIPLE of TileWidth
            //Load the Texture into memory
            //Itterate through all the tiles and store data
            //store a array of sprites
            //store a array of custom Tile (these tiles mainly will be the tile and then the GID in relation to this map).
            //Save all the sprites and custom tiles to the assetDatabase.
            //Create a new gameobject and add grid component to it.
            //create a child object and add tilemap renderer and tilemap to it
            //make sure the size of the tilemap is the same size as the length of the customTiles.
            //load the array of tiles into the tilemap on the child object.
            //save that gameobject to the database as a prefab.
            //for each tileset try and put it in a different folder and then retain a record of that folder location.
            //then if I try to clear this in the future I could just delete the entire folder.

            if(!list.isArray)
                return;

            int Counter = 0;
            foreach (LocalTileSet tileSet in map.TileSets)
            {
                if (!ValidateReadWrite(tileSet.TileSetImage))
                {
                    Debug.LogWarning($"{tileSet.TileSetImage.name} has not got Read/Write enabled [Skipping this TileSet]");
                    continue;
                }

                if (!TileSetTextureMultipleOfX(tileSet, out _))
                {
                    continue;
                }

                string orginalPath = AssetDatabase.GetAssetPath(tileSet.TileSetImage);
                CreateFolderStructure(tileSet.TileSetImage.name, out string m_path,
                    out string t_path);
                string newPath = $"{m_path}/{tileSet.TileSetImage.name}.png";
                if (!File.Exists($"{Application.dataPath.Replace("Assets", "")}{newPath}"))
                {
                    if (!string.IsNullOrEmpty(AssetDatabase.MoveAsset(orginalPath, newPath)))
                    {
                        Debug.LogWarning(
                            $"Could not move {tileSet.TileSetImage.name} to {m_path}/{tileSet.TileSetImage.name}.png");
                        continue;
                    }
                }

                AssetDatabase.SaveAssets(); //Might not need

                if (!SliceSprite(newPath))
                {
                    Debug.LogWarning($"Failed to slice up {newPath} skipping");
                    continue;
                }

                if (!SaveSlicedAssetsToAtlas(newPath))
                {
                    Debug.LogWarning($"Failed to save sliced up {newPath} to MasterAtlas skipping");
                    continue;
                }
                int x = DivisionInto(tileSet.TileSetImage.width, map.TileWidth);
                int y = DivisionInto(tileSet.TileSetImage.height, map.TileWidth);

                if (GenerateTiles(t_path, tileSet.TileSetImage.name, x * y, tileSet.FirstGID, x, y,
                        out TileBase[] tiles))
                {
                    GameObject RootObj = new GameObject($"{tileSet.TileSetImage.name}_TilePalette");
                    RootObj.AddComponent<UnityEngine.Grid>();
                    GameObject childObj = new GameObject("Layer");
                    childObj.transform.SetParent(RootObj.transform);
                    Tilemap tilemap = childObj.AddComponent<Tilemap>();
                    childObj.AddComponent<TilemapRenderer>();

                    tilemap.size = new Vector3Int(x, y, 1);
                    tilemap.origin = Vector3Int.zero;
                    tilemap.SetTilesBlock(tilemap.cellBounds, tiles);
                
                    // AssetDatabase.CreateAsset(RootObj, $"{m_path}/{RootObj.name}.asset");
                    bool prefabSuccess;
                    PrefabUtility.SaveAsPrefabAssetAndConnect(RootObj, $"{m_path}/{RootObj.name}.prefab",
                        InteractionMode.UserAction, out prefabSuccess);

                    if (!prefabSuccess)
                    {
                        Debug.LogWarning($"Failed to create {RootObj.name} prefab");
                        Destroy(RootObj);
                        continue;
                    }
                
                    //map.Active_FolderPaths.Add(m_path);
                    list.InsertArrayElementAtIndex(Counter);
                    list.GetArrayElementAtIndex(Counter).stringValue = m_path;
                    Counter++;
                }
                else
                {
                    Debug.LogWarning($"Failed To Generate Tiles for {tileSet.TileSetImage.name} skipping");
                    continue;
                }
            }
            AssetDatabase.SaveAssets();
            
        }

        private bool GenerateTiles(string Tilepath,string SourceTextureName,int length,int startingGID, int row, int column, out TileBase[] tiles)
        {
            tiles = new TileBase[length];
            SpriteAtlas atlas = map.AtlasAsset.GetMasterAtlas();
            if (atlas == null)
                return false;
            int frame = 0;
            for (int x = 0; x < row; x++)
            {
                for (int y = 0; y < column; y++)
                {
                    RecordedTile tile = ScriptableObject.CreateInstance<RecordedTile>();
                    tile.Gid = Convert.ToUInt16(startingGID);
                    Sprite TileSprite = atlas.GetSprite($"{SourceTextureName}_{frame}");
                    if (TileSprite == null)
                    {
                        frame++;
                        continue;
                    }

                    tile.Sprite = TileSprite;
                    tiles[row * x + y] = tile;
                    AssetDatabase.CreateAsset(tiles[row * x + y], $"{Tilepath}/{tile.Gid.ToString()}.asset");
                    frame++;
                    startingGID += 1;
                }
            }

            return true;
        }

        private bool SaveSlicedAssetsToAtlas(string texturePath)
        {
            map.AtlasAsset = SpriteAtlasAsset.Load("Assets/WaterNavTiled/MasterAtlas.spriteatlasv2");
            if (map.AtlasAsset == null)
            {
                map.AtlasAsset = new SpriteAtlasAsset();
            }
            SpriteAtlas spriteAtlas =
                AssetDatabase.LoadAssetAtPath<SpriteAtlas>("Assets/WaterNavTiled/MasterAtlas.spriteatlas");
            if (spriteAtlas == null)
                return false;
            if (!Equals(spriteAtlas, map.AtlasAsset.GetMasterAtlas()))
            {
                map.AtlasAsset.SetMasterAtlas(spriteAtlas);
            }
            try
            {
                Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();
                map.AtlasAsset.Add(sprites);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                return false;
            }
            
            SpriteAtlasAsset.Save(map.AtlasAsset, "Assets/WaterNavTiled/MasterAtlas.spriteatlasv2");
            return true;
        }

        private bool SliceSprite(string texturePath)
        {
            TextureImporter txImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (txImporter == null)
                return false;

            txImporter.textureType = TextureImporterType.Sprite;
            txImporter.spriteImportMode = SpriteImportMode.Multiple;
            txImporter.spritePixelsPerUnit = map.TileWidth;
            txImporter.filterMode = FilterMode.Point;
            txImporter.wrapMode = TextureWrapMode.Clamp;
            txImporter.maxTextureSize = 2048;
            txImporter.textureCompression = TextureImporterCompression.Uncompressed;
            txImporter.crunchedCompression = false;
            txImporter.compressionQuality = 100;
            txImporter.isReadable = true;
            txImporter.textureShape = TextureImporterShape.Texture2D;
            txImporter.npotScale = TextureImporterNPOTScale.None;
            
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

            Texture2D sourceTexture =  AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

            List<SpriteMetaData> spriteMetaDatas = new List<SpriteMetaData>();
            int frameNumber = 0;
            NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
            NativeArray<Color32> TextureData = sourceTexture.GetPixelData<Color32>(0);
            for (int y = sourceTexture.height; y > 0; y -= map.TileWidth)
            {
                for (int x = 0; x < sourceTexture.width; x += map.TileWidth)
                {
                    CheckTextureRegionJob job = new CheckTextureRegionJob();
                    job.result = result;
                    job.height = map.TileWidth;
                    job.width = map.TileWidth;
                    job.xpos = y * map.TileWidth;
                    job.ypos = x * map.TileWidth;
                    job.TextureData = TextureData;
                    
                    JobHandle handle = job.Schedule();
                    handle.Complete();
                    
                    if (result[0] != 0)
                    {
                        SpriteMetaData spriteMetaData = new SpriteMetaData()
                        {
                            name = $"{sourceTexture.name}_{frameNumber}",
                            rect = new Rect(new Vector2(x, y - map.TileWidth),
                                new Vector2(map.TileWidth, map.TileWidth)),
                            alignment = 0,
                            pivot = Vector2.zero
                        };
                        spriteMetaDatas.Add(spriteMetaData);
                        frameNumber++;
                    }
                }
            }

            result.Dispose();
            TextureData.Dispose();
            Debug.Log($"Sliced {spriteMetaDatas.Count}");
            txImporter.spritesheet = spriteMetaDatas.ToArray(); //TODO the spritesheet does not seem to be saving which is why it's causing problems with the tile creation
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
            return true;
        }

        private void CreateFolderStructure(string tileSetName, out string mainFolderPath, out string tileFolderPath)
        {
            if (AssetDatabase.IsValidFolder($"Assets/WaterNavTiled/{tileSetName}_Folder"))
            {
                mainFolderPath = $"Assets/WaterNavTiled/{tileSetName}_Folder";
            }
            else
            {
                string FolderGuid =
                    AssetDatabase.CreateFolder("Assets/WaterNavTiled", $"{tileSetName}_Folder");
                mainFolderPath = AssetDatabase.GUIDToAssetPath(FolderGuid); 
            }

            if (AssetDatabase.IsValidFolder($"{mainFolderPath}/Tiles_Folder"))
            {
                tileFolderPath = $"{mainFolderPath}/Tiles_Folder";
            }
            else
            {
                string TileGUID = AssetDatabase.CreateFolder(mainFolderPath, $"Tiles_Folder");
                tileFolderPath = AssetDatabase.GUIDToAssetPath(TileGUID);
            }
        }

        private bool ValidateReadWrite(Texture tex) => tex.isReadable;
        
        private void Validate(SerializedProperty list)
        {
            if (list.isArray)
            {
                ValidateTileSets(list);
            }
        }

        private void ValidateTileSets(SerializedProperty list)
        {
            //Go through each TileSet
            //Grab that texture
            //See if it has readvalue fine
            //itterate how many Tilewidth X Tilewidth blocks can fit in that image size
            //The NEXT First GID for the next Tileset is the last value + 1

            if (list.arraySize > 0)
            {
                int nextFirstGID = 0;

                for (int i = 0; i < list.arraySize; i++)
                {
                    object RefrenceValue = list.GetArrayElementAtIndex(i).FindPropertyRelative("TileSetImage")
                        .objectReferenceValue;
                    
                    if (RefrenceValue ==
                        null)
                    {
                        continue;
                    }
                    list.GetArrayElementAtIndex(i).FindPropertyRelative("FirstGID").intValue = nextFirstGID;
                    if (TileSetTextureMultipleOfX(RefrenceValue, out int largestLocalID))
                    {
                        nextFirstGID += largestLocalID + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                
                // foreach (LocalTileSet tileSet in map.TileSets)
                // {
                //     if (tileSet.TileSetImage == null)
                //     {
                //         Counter++;
                //         continue;
                //     }
                //     //tileSet.FirstGID = nextFirstGID;
                //     list.GetArrayElementAtIndex(Counter).FindPropertyRelative("FirstGID").intValue = nextFirstGID;
                //     if (TileSetTextureMultipleOfX(list.GetArrayElementAtIndex(Counter).objectReferenceValue, out int largestLocalID))
                //     {
                //         nextFirstGID += largestLocalID + 1;
                //         Counter++;
                //     }
                //     else
                //     {
                //         Counter++;
                //         break;
                //     }
                // }
            }
            
            // if (map.TileSets.Length > 0)
            // {
            //     int nextFirstGID = 0;
            //     map.TileSets[0].FirstGID = 0;
            //     foreach (LocalTileSet tileSet in map.TileSets)
            //     {
            //         if(tileSet.TileSetImage == null)
            //             continue;
            //         tileSet.FirstGID = nextFirstGID;
            //         if (TileSetTextureMultipleOfX(tileSet, out int largestLocalID))
            //         {
            //             nextFirstGID += largestLocalID + 1;
            //         }
            //         else
            //         {
            //             break;
            //         }
            //     }
            // }
        }
        
        private  int DivisionInto(int total, int divisionnumber)
        {
            return total / divisionnumber;
        }
        private bool IsXMultipleOfY(int a, int b)
        {
            return a % b == 0;
        }

        private bool TileSetTextureMultipleOfX(LocalTileSet tileSet, out int largestID)
        {
            largestID = 0;
            if (!tileSet.TileSetImage.isReadable)
            {
                Debug.LogWarning($"{tileSet.TileSetImage.name} is not readable");
                return false;
            }

            if (!IsXMultipleOfY(tileSet.TileSetImage.width, map.TileWidth) ||
                !IsXMultipleOfY(tileSet.TileSetImage.height, map.TileWidth))
            {
                Debug.LogWarning($"{tileSet.TileSetImage.name} is not a multiple of {map.TileWidth}");
                return false;
            }
              

            int x = DivisionInto(tileSet.TileSetImage.width, map.TileWidth);
            int y = DivisionInto(tileSet.TileSetImage.height, map.TileWidth);
            largestID = (x * y) - QuantityofNullChunks(tileSet.TileSetImage, x, y);
            return true;
        }
        private bool TileSetTextureMultipleOfX(object tileSet, out int largestID)
        {
            largestID = 0;
            if (tileSet is Texture2D)
            {
                Texture2D T_set = tileSet as Texture2D;
                if (!T_set.isReadable)
                {
                    Debug.LogWarning($"{T_set.name} is not readable");
                    return false;
                }

                if (!IsXMultipleOfY(T_set.width, map.TileWidth) ||
                    !IsXMultipleOfY(T_set.height, map.TileWidth))
                {
                    Debug.LogWarning($"{T_set.name} is not a multiple of {map.TileWidth}");
                    return false;
                }
              

                int x = DivisionInto(T_set.width, map.TileWidth);
                int y = DivisionInto(T_set.height, map.TileWidth);
                largestID = (x * y) - QuantityofNullChunks(T_set, x, y);
                return true;
            }

            return false;
        }

        private int QuantityofNullChunks(Texture2D tex, int RowCount, int ColumnCount)
        {
            int Quantity = 0;
            NativeArray<Color32> TextureData = tex.GetPixelData<Color32>(0);
            NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
            for (int x = 0; x < RowCount; x++)
            {
                for (int y = 0; y < ColumnCount; y++)
                {
                    CheckTextureRegionJob job = new CheckTextureRegionJob();
                    job.result = result;
                    job.height = map.TileWidth;
                    job.width = map.TileWidth;
                    job.xpos = x * map.TileWidth;
                    job.ypos = y * map.TileWidth;
                    job.TextureData = TextureData;

                    JobHandle handle = job.Schedule();
                    handle.Complete();
                    if (result[0] == 0)
                        Quantity += 1;

                }
            }

            TextureData.Dispose();
            result.Dispose();
            return Quantity;
        }

        // private bool TextureRectNotTransparent(Texture2D tex, Rect area)
        // {
        //     
        // }
    }
    
#endif
}