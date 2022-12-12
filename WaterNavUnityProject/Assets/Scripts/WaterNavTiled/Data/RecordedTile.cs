using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaterNavTiled.Data
{
    /// <summary>
    /// Recorded Tiles are meant to be stored inside LocalTilesets each instance of a RecordedTile is basically a new tile
    /// </summary>
    
    [Serializable] 
    public class RecordedTile : TileBase
    {
        public UInt16 Gid;
        public Sprite Sprite;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = Sprite;
        }
    }
}