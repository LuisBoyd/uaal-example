using System;

namespace WaterNavTiled.Enums
{
    /// <summary>
    /// Based on 16 bit the first 3 HSB (Highest Significant bit) are flags at the moment
    /// </summary>

    [Flags]
    public enum RecordedTileFlags
    {
        LandPathfinding, //Land based AI can traverse this tile
        WaterPathFinding, //Water based AI can traverse this tile
        BuildableTile //if I can place stuff on top of this tile mainly later down the line with building decorations
    }
}