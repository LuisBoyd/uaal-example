using System;

namespace RCRCoreLib.Core.Tiles.TilemapSystem
{
    [Flags]
    public enum WorldTileEffectFlags : short
    {
        None = 0,
        WaterPathFindable = 1,
        LandPathFindable = 2,
        StructureOccupied = 4,
    }
}