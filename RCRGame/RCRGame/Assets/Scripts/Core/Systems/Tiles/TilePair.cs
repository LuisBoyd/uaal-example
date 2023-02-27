using System;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Systems.Tiles
{
    [Serializable]
    public class TilePair
    {
        public TileSelectionOptions Category;
        public TileBase LogicTile;
        public TileBase VisualTile;
    }
}