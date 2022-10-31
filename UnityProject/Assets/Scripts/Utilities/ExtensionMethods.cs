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

                        Dictionary<int, int> tileInstanceID = new Dictionary<int, int>();
                        byte[] tileArrayInt = new Byte[tileArray.Length];

                        for (int i = 0; i < tileArray.Length; i++)
                        {
                            if (tileArray[i] != null)
                            {
                                int id = tileArray[i].GetInstanceID();
                                if(!tileInstanceID.ContainsKey(id))
                                    tileInstanceID.Add(id, tileInstanceID.Count + 1);

                                try
                                {
                                    tileArrayInt[i] = Convert.ToByte(tileInstanceID[id]);
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

                        List<string> relevantTileAssetPaths = new List<string>();

                        foreach (KeyValuePair<int,int> tileID in tileInstanceID)
                        {
                            relevantTileAssetPaths.Add(AssetDatabase.GetAssetPath(tileID.Key));
                        }

                        MapData mData = new MapData()
                        {
                            TileArray = tileArrayInt, TileAssetPaths = relevantTileAssetPaths,
                            X = tilemap.size.x, Y = tilemap.size.y
                        };

                       

                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(writer, mData);
                    }

                    if (!FileManager.WriteToFile(path, Convert.ToBase64String(ms.ToArray()), true))
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
    }