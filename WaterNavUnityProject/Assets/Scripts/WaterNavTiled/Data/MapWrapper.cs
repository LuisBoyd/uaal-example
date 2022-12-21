using System.Collections.Generic;
using Newtonsoft.Json;
using RCR.Tiled;
using UnityEngine;
using WaterNavTiled.CustomConverters;

namespace WaterNavTiled.Data
{
    [JsonConverter(typeof(MapWrapperJsonConverter))]
    public class MapWrapper
    {
        public List<LocalTileSet> TileSets { get; set; }
        public List<Layer> Layers { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TileWidth { get; set; }

        public MapWrapper(IEnumerable<LocalTileSet> tileSets,
            IEnumerable<Layer> layers, Vector2Int mapSize,
            int tileWidth)
        {
            TileSets = new List<LocalTileSet>();
            Layers = new List<Layer>();
            foreach (var localTileSet in tileSets)
                TileSets.Add(localTileSet);
            foreach (var layer in layers)
                Layers.Add(layer);
            X = mapSize.x;
            Y = mapSize.y;
            TileWidth = tileWidth;

        }
    }
}