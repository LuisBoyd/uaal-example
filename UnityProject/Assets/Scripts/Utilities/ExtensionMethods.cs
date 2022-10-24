using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Enums;
using RCR.ScriptableObjects;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace RCR.Utilities
{
    public static class ExtensionMethods
    {
        private static readonly HttpClient client = new HttpClient();
        
        public static async void serializeTilemap(this Tilemap tilemap)
        {
            TileBase[] bases = tilemap.GetTilesBlock(tilemap.cellBounds);
            TileType[] types = new TileType[bases.Length];
            for (int i = 0; i < bases.Length; i++)
            {
                if (bases[i] is SerializableTile)
                {
                    types[i] = (bases[i] as SerializableTile).TileType;
                }
                else
                {
                    types[i] = TileType.None;
                }
            }

            byte[] bytearray = new byte[sizeof(int) * types.Length];
            using (MemoryStream stream = new MemoryStream(bytearray))
            {
                // using (BinaryWriter binaryWriter = new BinaryWriter(stream))
                // {
                //     stream.Position = 0;
                //     for (int i = 0; i < types.Length; i++)
                //     {
                //         int j = Convert.ToInt32(types[i]);
                //         if (j != 0)
                //         {
                //             
                //         }
                //         binaryWriter.Write(j);
                //     }
                //
                //     long t = stream.Length;
                //    int k = stream.Read(bytearray, 0, bytearray.Length);
                // }

                // using (BsonDataWriter bson = new BsonDataWriter(stream))
                // {
                //     var jsonseriliazer = new JsonSerializer();
                //     
                //     //jsonseriliazer.Serialize(bson,);
                // }
                
            }
            
            
            
        }
    }
}