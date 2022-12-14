using System;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaterNavTiled.Interfaces;

namespace WaterNavTiled.Data
{
    /// <summary>
    /// Recorded Tiles are meant to be stored inside LocalTilesets each instance of a RecordedTile is basically a new tile
    /// </summary>
    
    [Serializable] 
    public class RecordedTile : TileBase, IJsonSerializable
    {
        public UInt16 Gid;
        public Sprite Sprite;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = Sprite;
        }

        public void GetObjectData(BsonDataWriter info)
        {
            info.WriteStartObject();
            info.WritePropertyName("Gid");
            info.WriteValue(Gid);
            
            info.WritePropertyName("x");
            info.WriteValue(Sprite.rect.x);
            info.WritePropertyName("y");
            info.WriteValue(Sprite.rect.y);
            info.WritePropertyName("width");
            info.WriteValue(Sprite.rect.width);
            info.WritePropertyName("height");
            info.WriteValue(Sprite.rect.height);
            
            info.WriteEndObject();
        }
    }
}