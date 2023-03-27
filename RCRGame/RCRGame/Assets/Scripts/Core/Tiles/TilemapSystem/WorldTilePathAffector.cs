using System;

namespace RCRCoreLib.Core.Tiles.TilemapSystem
{
    [Flags]
    public enum WorldTilePathAffector : short
    {
        None = 0,
        WaterPathFindable = 1,
        LandPathFindable = 2,
    }
}