using System.Collections.Generic;
using System.Linq;

namespace RCRMapLibrary
{
    public static class TileTypes
    {
        private static readonly Dictionary<string, TileType> _TileTypes;

        static TileTypes()
        {
            _TileTypes = new Dictionary<string, TileType>();
        }

        public static void AddNewTileType(string name) => _TileTypes.Add(name,new TileType(name));

        public static void AddNewTileType(string name, string source)
        {
            TileType type = new TileType(name);
            type.Source = source;
            _TileTypes.Add(name, type);
        }

        public static bool GetTileType(string name, out TileType tileType)
        {
            tileType = null;
            if (!_TileTypes.ContainsKey(name))
                return false;
            tileType = _TileTypes[name];
            return true;
        }

        public static bool GetTileTypeBySource(string source, out TileType tileType)
        {
            tileType = _TileTypes.Values.First(x => x.Source.Equals(source));
            if (tileType == null)
                return false;
            return true;
        }
    }
}