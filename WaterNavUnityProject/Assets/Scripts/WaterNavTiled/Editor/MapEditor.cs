using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RCR.Tiled;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEditor.U2D;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using WaterNavTiled.Data;
using WaterNavTiled.Editor.Jobs;
using Object = UnityEngine.Object;
using UtitliesEditor = RCR.editor.EditorUtilities;

namespace WaterNavTiled.Editor
{
#if UNITY_EDITOR
    
    [CustomEditor(typeof(Map))]
    public class MapEditor : UnityEditor.Editor
    {
        private Map map {get => target as Map;}

        private int PathCount;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            
            if(GUILayout.Button("GENERATE"))
                SetUpMap();

            EditorGUILayout.Space(25f);
            if (GUILayout.Button("TILESETS 2 UNITY"))
                Tilesets2Unity();
                
            EditorGUILayout.Space(25f);
            if (GUILayout.Button("Parse"))
                ParseTilemap();
            EditorGUILayout.Space(25f);
            
            //Map Settings
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MapSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TileWidth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Destination"));
            
            EditorGUILayout.Space(25f);
            
            EditorList.Show(serializedObject.FindProperty("TileSets"), EditorListOption.Buttons);
            

            if (EditorGUI.EndChangeCheck())
            {
                Validate(serializedObject.FindProperty("TileSets"));
                EditorUtility.SetDirty(map);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ParseTilemap()
        {
            map.ParseMap();
        }

        private void SetUpMap()
        {
            map.SaveMap();
        }

        private void Tilesets2Unity()
        {
            int Counter = 0;
            foreach (LocalTileSet tileSet in map.TileSets)
            {
                if (!ValidateReadWrite(tileSet.TileSetImage))
                {
                    Debug.LogWarning($"{tileSet.TileSetImage.name} has not got Read/Write enabled [Skipping this TileSet]");
                    continue;
                }

                if (!TileSetTextureMultipleOfX(tileSet))
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
                int x = DivisionInto(tileSet.TileSetImage.width, map.TileWidth);
                int y = DivisionInto(tileSet.TileSetImage.height, map.TileWidth);
                SerializedProperty localTileSet = serializedObject.FindProperty("TileSets");
                TileBase[] tiles = new TileBase[x * y];
                if (GenerateTiles(t_path, tileSet.TileSetImage.name, newPath, tileSet.FirstGID,
                        localTileSet.GetArrayElementAtIndex(Counter).FindPropertyRelative("RecordedTiles"),ref tiles))
                {
                    if (!File.Exists(
                            $"{Application.dataPath.Replace("Assets", "")}{m_path}/{tileSet.TileSetImage.name}_TilePalette.prefab"))
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
                    
                        bool prefabSuccess;
                        PrefabUtility.SaveAsPrefabAssetAndConnect(RootObj, $"{m_path}/{RootObj.name}.prefab",
                            InteractionMode.UserAction, out prefabSuccess);

                        if (!prefabSuccess)
                        {
                            Debug.LogWarning($"Failed to create {RootObj.name} prefab");
                            Destroy(RootObj);
                            continue;
                        }
                    }
                    if (localTileSet != null && localTileSet.isArray)
                    {
                        SerializedProperty localTileSetPath = localTileSet.GetArrayElementAtIndex(Counter)
                            .FindPropertyRelative("FilePath");
                        // if (!PathExists && localTileSetPath != null)
                        // {
                        //     list.InsertArrayElementAtIndex(Counter);
                        //     list.GetArrayElementAtIndex(Counter).stringValue = m_path;
                        //     localTileSetPath.stringValue = m_path;
                        //     Counter++;
                        // }
                        if (localTileSetPath != null)
                        {
                            localTileSetPath.stringValue = m_path;
                            Counter++;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Failed To Generate Tiles for {tileSet.TileSetImage.name} skipping");
                    continue;
                }
            }
            AssetDatabase.SaveAssets();
            
        }

        private bool GenerateTiles(string Tilepath,string SourceTextureName,string TexPath,int startingGID,SerializedProperty set, ref TileBase[] tiles)
        {
            // tiles = new TileBase[length];
            // SpriteAtlas atlas = map.AtlasAsset.GetMasterAtlas();
            // if (atlas == null)
            //     return false;
            // int frame = 0;
            // for (int x = 0; x < row; x++)
            // {
            //     for (int y = 0; y < column; y++)
            //     {
            //         RecordedTile tile = ScriptableObject.CreateInstance<RecordedTile>();
            //         tile.Gid = Convert.ToUInt16(startingGID);
            //         Sprite TileSprite = atlas.GetSprite($"{SourceTextureName}_{frame}");
            //         if (TileSprite == null)
            //         {
            //             frame++;
            //             continue;
            //         }
            //
            //         tile.Sprite = TileSprite;
            //         tiles[row * x + y] = tile;
            //         AssetDatabase.CreateAsset(tiles[row * x + y], $"{Tilepath}/{tile.Gid.ToString()}.asset");
            //         frame++;
            //         startingGID += 1;
            //     }
            // }
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(TexPath).OfType<Sprite>().ToArray();
            if (sprites.Length <= 0)
            {
                tiles = null;
                return false;
            }

            if (!set.isArray)
            {
                Debug.LogWarning($"{set.name} is not an array, saving recorded tiles to associated tilesets will not happen");
                return false;
            }

            for (int i = 0; i < set.arraySize; i++)
            {
                set.DeleteArrayElementAtIndex(i);
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                UInt16 GID = Convert.ToUInt16(startingGID);
                RecordedTile tile = null;
                if (!File.Exists($"{Application.dataPath.Replace("Assets", "")}{Tilepath}/{GID.ToString()}.asset"))
                {
                    if (sprites[i].rect.width > map.TileWidth || sprites[i].rect.height > map.TileWidth)
                    {
                        Debug.LogWarning($"{sprites[i].name} sprite was width or height was larger than {map.TileWidth} \n Skipping Creating and registering this tile");
                        continue;
                    }
                    tile = ScriptableObject.CreateInstance<RecordedTile>();
                    tile.Gid = GID;
                    tile.Sprite = sprites[i];
                    AssetDatabase.CreateAsset(tile, $"{Tilepath}/{tile.Gid.ToString()}.asset");
                }
                else
                {
                    tile = AssetDatabase.LoadAssetAtPath<RecordedTile>($"{Tilepath}/{GID.ToString()}.asset");
                    if (tile == null)
                    {
                        Debug.LogWarning($"Problem With Getting RecordedTile from AssetDatabase {Tilepath}/{GID.ToString()}.asset \n" +
                                         $"Skipping registering this tile");
                        continue;
                    }else if (tile.Sprite.rect.width > map.TileWidth || tile.Sprite.rect.height > map.TileWidth)
                    {
                        Debug.LogWarning($"{tile.name} sprite was width or height was larger than {map.TileWidth} \n Skipping registering this tile");
                        continue;
                    }
                }
                set.InsertArrayElementAtIndex(i);
                set.GetArrayElementAtIndex(i).objectReferenceValue = tile;
                tiles[i] = tile;
                startingGID += 1;
            }
            AssetDatabase.SaveAssets();
            return true;
        }

        [Obsolete]
        private bool SaveSlicedAssetsToAtlas(string texturePath)
        {
            SerializedProperty AtlasAsset = serializedObject.FindProperty("AtlasAsset");
            SpriteAtlasAsset spriteAtlasAsset = SpriteAtlasAsset.Load("Assets/WaterNavTiled/MasterAtlas.spriteatlasv2");

            if (spriteAtlasAsset == null)
            {
                spriteAtlasAsset = new SpriteAtlasAsset();
            }

            if (UtitliesEditor.SaveRefrenceValueToSerializedProperty(ref AtlasAsset, spriteAtlasAsset))
            {
                SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>("Assets/WaterNavTiled/MasterAtlas.spriteatlas");
                if (spriteAtlas == null)
                    return false;
                if (UtitliesEditor.IsSerializedPropertyX<SpriteAtlasAsset>(AtlasAsset, out spriteAtlasAsset))
                {
                    if (!Equals(spriteAtlas, spriteAtlasAsset.GetMasterAtlas()))
                    {
                        spriteAtlasAsset.SetMasterAtlas(spriteAtlas);
                    }

                    try
                    {
                        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();
                        spriteAtlasAsset.Add(sprites);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                        return false;
                    }
                    
                    if (!UtitliesEditor.SaveRefrenceValueToSerializedProperty(ref AtlasAsset, spriteAtlasAsset))
                    {
                        return false;
                    }
                    SpriteAtlasAsset.Save(spriteAtlasAsset,"Assets/WaterNavTiled/MasterAtlas.spriteatlasv2");
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        [Obsolete]
        private bool SliceSprite(string texturePath)
        {
            TextureImporter txImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (txImporter == null)
                return false;

            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(txImporter);
            dataProvider.InitSpriteEditorDataProvider();
            var SpriteNameFileIDDataProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
            var spriteRects = dataProvider.GetSpriteRects().ToList();
            var nameFileIdPairs = SpriteNameFileIDDataProvider.GetNameFileIdPairs().ToList();
            spriteRects.Clear();
            nameFileIdPairs.Clear();
            Texture2D sourceTexture =  AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            int frameNumber = 0;
            NativeArray<int> result = new NativeArray<int>((map.TileWidth * map.TileWidth), Allocator.TempJob);
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
                    
                    if (result.Any(z => z != 0))
                    {
                        SpriteRect spriteRect = new SpriteRect()
                        {
                            name = $"{sourceTexture.name}_{frameNumber}",
                            rect = new Rect(new Vector2(x, y - map.TileWidth),
                                new Vector2(map.TileWidth, map.TileWidth)),
                            alignment = 0,
                            pivot = Vector2.zero,
                            spriteID = GUID.Generate()
                        };
                        //spriteMetaDatas.Add(spriteMetaData);
                        nameFileIdPairs.Add(new SpriteNameFileIdPair(spriteRect.name, spriteRect.spriteID));
                        spriteRects.Add(spriteRect);
                        frameNumber++;
                    }
                }
            }
            result.Dispose();
            TextureData.Dispose();
            SpriteNameFileIDDataProvider.SetNameFileIdPairs(nameFileIdPairs.ToArray());
            dataProvider.SetSpriteRects(spriteRects.ToArray());
            dataProvider.Apply();
            var assetImporter = dataProvider.targetObject as AssetImporter;
            assetImporter.SaveAndReimport();
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
            if (list.arraySize > 0)
            {
                int nextFirstGID = 1;

                for (int i = 0; i < list.arraySize; i++)
                {
                    Texture2D texture2D = null;
                    UtitliesEditor.IsSerializedPropertyX<Texture2D>(
                        list.GetArrayElementAtIndex(i).FindPropertyRelative("TileSetImage"),
                        out texture2D);
                    
                    if (texture2D ==
                        null)
                    {
                        continue;
                    }
                    list.GetArrayElementAtIndex(i).FindPropertyRelative("FirstGID").intValue = nextFirstGID;
                    if (GetTileSetSpriteCount(texture2D, out int largestLocalID))
                    {
                        nextFirstGID += largestLocalID + 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
        
        private  int DivisionInto(int total, int divisionnumber)
        {
            return total / divisionnumber;
        }
        private bool IsXMultipleOfY(int a, int b)
        {
            return a % b == 0;
        }

        private bool TileSetTextureMultipleOfX(LocalTileSet tileSet)
        {
            
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
            return true;
        }

        private bool GetTileSetSpriteCount(Texture2D tex, out int quantity)
        {
            quantity = 0;
            string TexPath = AssetDatabase.GetAssetPath(tex);
            AssetImporter A_importer = AssetImporter.GetAtPath(TexPath);
            if (A_importer == null)
                return false;
            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(A_importer);
            dataProvider.InitSpriteEditorDataProvider();
            quantity = dataProvider.GetSpriteRects().Length;
            return true;
        }
        
        [Obsolete]
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

        [Obsolete]
        private int QuantityofNullChunks(Texture2D tex, int RowCount, int ColumnCount)
        {
            int Quantity = 0;
            NativeArray<Color32> TextureData = tex.GetPixelData<Color32>(0);
            NativeArray<int> result = new NativeArray<int>((map.TileWidth * map.TileWidth), Allocator.TempJob);
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
                    if (result.Any(z => z != 0))
                        Quantity += 1;

                }
            }

            TextureData.Dispose();
            result.Dispose();
            return Quantity;
        }
    }
    
#endif
}