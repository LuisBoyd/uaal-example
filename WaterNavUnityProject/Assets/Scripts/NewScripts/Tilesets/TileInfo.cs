using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCR.Settings.NewScripts.Tilesets
{
    public class TileInfo : ITileInfo
    {
        public Tilemap Tilemap { get; }
        public Vector2Int WorldLocation { get; }

        public TileInfo(Vector2Int worldLocation, ref Tilemap tilemap)
        {
            WorldLocation = worldLocation;
            Tilemap = tilemap;
        }
    }
}