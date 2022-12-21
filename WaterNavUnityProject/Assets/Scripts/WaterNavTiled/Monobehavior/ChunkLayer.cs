using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaterNavTiled.Data;

namespace WaterNavTiled
{
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    public class ChunkLayer : Layer
    {
        [SerializeField] private int Height = 50;
        [SerializeField] private int Width = 50;
        [SerializeField] private int XPos;
        [SerializeField] private int YPos;
        private UInt16[] Data;
        
        public override void CollectMonoData()
        {
            if (transform.parent.GetComponent<TileLayer>() == null)
            {
                Debug.LogWarning("Chunk Layers can only exist under TileLayers");
                return;
            }
            Tilemap map = GetComponent<Tilemap>();
            BoundsInt ChunkBounds = new BoundsInt(new Vector3Int(XPos, YPos,0), new Vector3Int(Width, Height, 1));
            TileBase[] tiles = map.GetTilesBlock(ChunkBounds);
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

        public override void GetObjectData(JsonWriter info)
        {
            info.WriteStartObject();
            info.WritePropertyName("Data");
            info.WriteStartArray();
            for (int i = 0; i < Data.Length; i++)
            {
                info.WriteValue(Data[i]);
            }
            info.WriteEndArray();
            info.WritePropertyName("Height");
            info.WriteValue(Height);
            info.WritePropertyName("Width");
            info.WriteValue(Width);
            info.WritePropertyName("X");
            info.WriteValue(XPos);
            info.WritePropertyName("Y");
            info.WriteValue(YPos);
            info.WriteEndObject();
        }
    }
}