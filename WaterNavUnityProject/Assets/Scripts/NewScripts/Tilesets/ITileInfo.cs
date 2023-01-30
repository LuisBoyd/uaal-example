using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCR.Settings.NewScripts.Tilesets
{
    public interface ITileInfo
    { 
        public Tilemap Tilemap { get; }
        public Vector2Int WorldLocation { get; }
    }
}