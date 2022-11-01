using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using DataStructures;
using RCR.ScriptableObjects;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using RCR.Enums;
using RCR.Managers;
using UnityEditor;
using UnityEngine;

namespace RCR.Utilities
{
    public static class ExtensionMethods
    {
        public static void SerializeTilemap(this Tilemap tilemap, string assetBundlePath)
        {
            const int X = 50;
            const int Y = 50;
            tilemap.CompressBounds();
            if (tilemap.size.x != X || tilemap.size.y != Y)
            {
                Debug.LogWarning($"The tilemap is not the correct dimensions based on constant value {X} and {Y}");
                tilemap.origin = Vector3Int.zero;
                tilemap.size = new Vector3Int(50, 50, 1);
                tilemap.ResizeBounds();
            }

            List<string> Dependinces;
            if (!File.Exists(FileManager.MapPartitionManifest))
            {
                FileManager.WriteToFile(FileManager.MapPartitionManifest, "", true);
            }

            if (FileManager.LoadFromFile(FileManager.MapPartitionManifest, out string data, true))
            {
                if (string.IsNullOrEmpty(data))
                {
                    Dependinces = new List<string>();
                }
                else
                {
                    Dependinces = JsonConvert.DeserializeObject<List<string>>(data);
                    if (Dependinces == null)
                    {
                        Debug.LogError("Could Not Generate Dependicnes");
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError($"Could not load manifest {FileManager.MapPartitionManifest}");
                return;
            }
            
            string path = EditorUtility.SaveFilePanel("Save Tilemap as BSON", "", "Tilemap.bmap", "bmap");
            if (tilemap != null && !string.IsNullOrEmpty(path))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BsonDataWriter writer = new BsonDataWriter(ms))
                    {
                        TileBase[] tileArray = tilemap.GetTilesBlock(tilemap.cellBounds);
                        
                        byte[] tileArrayInt = new Byte[tileArray.Length];

                        for (int i = 0; i < tileArray.Length; i++)
                        {
                            if (tileArray[i] != null)
                            {
                                string id = AssetDatabase.GetAssetPath(tileArray[i]);
                                if (TileManager.m_AssetDatabaseLookup.ContainsKey(id))
                                {
                                    try
                                    {
                                        tileArrayInt[i] = Convert.ToByte(TileManager.m_AssetDatabaseLookup[id]);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                        throw;
                                    }
                                }
                                else
                                {
                                    tileArrayInt[i] = 0;
                                }
                            }
                            else
                            {
                                tileArrayInt[i] = 0;
                            }
                        }

                        // List<string> relevantTileAssetPaths = new List<string>();
                        //
                        // foreach (KeyValuePair<int,int> tileID in tileInstanceID)
                        // {
                        //     relevantTileAssetPaths.Add(AssetDatabase.GetAssetPath(tileID.Key));
                        // }
                        //
                        // MapData mData = new MapData()
                        // {
                        //     TileArray = tileArrayInt
                        // };
                        //
                        //
                        //
                        // JsonSerializer serializer = new JsonSerializer();
                        // serializer.Serialize(writer, mData);
                        
                        if (!FileManager.WriteToFile(path, Convert.ToBase64String(tileArrayInt), true))
                        {
                            Debug.LogError($"Targeted Tilemap could not be converted to a Json Format {tilemap.name}");
                            return;
                        }
                        else
                        {
                            if (!Dependinces.Contains(path))
                            {
                                Dependinces.Add(path);
                                FileManager.WriteToFile(FileManager.MapPartitionManifest, JsonConvert.SerializeObject(Dependinces), true);
                            }
                        }
                    }

                  
                }
            }
            
        }

        public static async void DeserializeTilemap(this Tilemap tilemap)
        {
            string path = EditorUtility.OpenFilePanel("Open Tilemap File", "", "bmap");
            if (tilemap != null && !string.IsNullOrEmpty(path))
            {
                char[] result;
                StringBuilder builder = new StringBuilder();
                using (StreamReader streamReader = File.OpenText(path))
                {
                    result = new char[streamReader.BaseStream.Length];
                    await streamReader.ReadAsync(result, 0, (int)streamReader.BaseStream.Length);
                }

                foreach (char character in result)
                {
                    builder.Append(character);
                }
                
                byte[] ReadBytes = Convert.FromBase64String(builder.ToString());
                TileType[] types = new TileType[ReadBytes.Length];
                for (int i = 0; i < ReadBytes.Length; i++)
                {
                    types[i] = (TileType)ReadBytes[i];
                }

                Dictionary<TileType, TileBase> lookup = new Dictionary<TileType, TileBase>();
                TileBase[] tiles = new TileBase[types.Length];
                for (int i = 0; i < tiles.Length; i++)
                {
                    if (!lookup.ContainsKey(types[i]))
                    {
                        if (TileManager.m_TileAddressable.TryGetValue(types[i], out string AssetPath))
                        {
                            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(AssetPath);
                            lookup.Add(types[i], tile);
                            tiles[i] = tile;
                        }
                    }
                    else
                    {
                        if (lookup.TryGetValue(types[i], out TileBase value))
                            tiles[i] = value;
                    }
                }

                if (Mathf.Sqrt(tiles.Length) <= tilemap.size.x && Mathf.Sqrt(tiles.Length) <= tilemap.size.y)
                {
                    tilemap.SetTilesBlock(tilemap.cellBounds, tiles);
                }
                else
                {
                    Debug.LogError($"Length of array {tiles.Length} bigger than the bounds of the tilemap");
                    Debug.Log($"Resized bounds to accomadate array length");
                    tilemap.size = new Vector3Int(Mathf.CeilToInt(Mathf.Sqrt(tiles.Length)),
                        Mathf.CeilToInt(Mathf.Sqrt(tiles.Length)));
                    tilemap.ResizeBounds();
                    tilemap.SetTilesBlock(tilemap.cellBounds, tiles);
                    return;
                }
                
            }
        }



    }
    }