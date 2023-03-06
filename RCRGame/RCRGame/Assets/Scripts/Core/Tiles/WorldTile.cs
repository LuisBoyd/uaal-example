using RCRCoreLib.Core.Tiles.TilemapSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Tiles
{
    [CreateAssetMenu(fileName = "New World Tile", menuName = "RCR/CustomTile/WorldTile", order = 0)]
    public class WorldTile : Tile
    {
        public WorldTileLockFlag lockFlag;
        public WorldTileEffectFlags effectFlags;
    }
}