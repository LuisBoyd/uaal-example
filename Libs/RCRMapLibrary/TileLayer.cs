using System.Collections.Generic;
using RCRCoreLibrary;

namespace RCRMapLibrary
{
    public class TileLayer : Layer
    {
        public FlattenedArray<TileBase> TileArray { get; private set; }
        
        public HashSet<TileBase> UsedTiles { get; private set; }

        public string Name { get; set; }
        public HashSet<CustomProperty> Properties { get; set; }

        public int SortingOrder { get; set; }
        
        public TileLayer()
        {
            Properties = new HashSet<CustomProperty>();
            UsedTiles = new HashSet<TileBase>();
            TileArray = new FlattenedArray<TileBase>(LayerWidth, LayerHeight);
        }
        
        
        
        
    }
}