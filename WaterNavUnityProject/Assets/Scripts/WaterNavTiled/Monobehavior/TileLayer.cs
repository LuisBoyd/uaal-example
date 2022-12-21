using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaterNavTiled.Data;

namespace WaterNavTiled
{
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    public class TileLayer : Layer
    {
        //Array of Chunks??
        [SerializeField] private string Compression; //Could be a enum to represent this
        private ChunkLayer[] chunk_layer;
        private UInt16[] Data;
        [SerializeField] private string encoding; //Could represent with enum
        [SerializeField] private int rowCount; //Same as MapHeight
        [SerializeField] private int ColumnCount; //same as MapWidth

        public override void GetObjectData(JsonWriter info)
        {
           
            info.WriteStartObject();
            info.WritePropertyName("Classname");
            info.WriteValue(Classname);
            info.WritePropertyName("Id");
            info.WriteValue(Id);
            info.WritePropertyName("Locked");
            info.WriteValue(Locked);
            info.WritePropertyName("Name");
            info.WriteValue(Name);
            info.WritePropertyName("Opacity");
            info.WriteValue(Opacity);
            info.WritePropertyName("Type");
            info.WriteValue(Type);
            info.WritePropertyName("Visible");
            info.WriteValue(Visible);
            info.WritePropertyName("Compression");
            info.WriteValue(Compression);
            info.WritePropertyName("encoding");
            info.WriteValue(encoding);
            info.WritePropertyName("rowCount");
            info.WriteValue(rowCount);
            info.WritePropertyName("ColumnCount");
            info.WriteValue(ColumnCount);
            info.WritePropertyName("Data");
            info.WriteStartArray();
            for (int i = 0; i < Data.Length; i++)
            {
                info.WriteValue(Data[i]);
            }
            info.WriteEndArray();
            
            info.WritePropertyName("Chunks");
            info.WriteStartArray();
            foreach (ChunkLayer chunk in chunk_layer)
            {
                chunk.GetObjectData(info);
            }
            info.WriteEndArray();
            info.WriteEndObject();
        }

        public override void CollectMonoData()
        {
            chunk_layer = gameObject.GetComponentsInChildren<ChunkLayer>();
            foreach (var chunk in chunk_layer)
            {
                chunk.CollectMonoData();
            }
            
            Tilemap map = GetComponent<Tilemap>();
            TileBase[] tiles = map.GetTilesBlock(map.cellBounds);
            Data = new ushort[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] is RecordedTile)
                {
                    Data[i] = (tiles[i] as RecordedTile).Gid;
                }
                else
                {
                    Data[i] = 0;
                }
            }
        }
    }
}