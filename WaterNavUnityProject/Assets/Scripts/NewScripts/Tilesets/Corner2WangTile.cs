using System;

namespace RCR.Settings.NewScripts.Tilesets
{
    public class Corner2WangTile
    {
        /*Wang tilesets are usually edge tilesets. But we can also create a Wang tileset by
         * considering the tile corners. Each tile has four corners so for 2 different types of
         * corner we have 2^4 or 16 different tiles, the same number as 2 different edges.
         * http://www.cr31.co.uk/stagecast/wang/2corn.html
         */
        
        [Flags]
        public enum BitwiseTileIndex //with bitwise operations total of 16 (15 - base 0) options
        {
            NorthEast = 1,
            SouthEast = 2,
            SouthWest = 4,
            NorthWest = 8,
        }
        
        
    }
}