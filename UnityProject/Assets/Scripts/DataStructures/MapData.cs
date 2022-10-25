using System.Collections.Generic;

namespace DataStructures
{
    [System.Serializable]
    public class MapData
    {
        public List<string> TileAssetPaths;
        public int X = 0;
        public int Y = 0;
        public int[] TileArray = default;
    }
}