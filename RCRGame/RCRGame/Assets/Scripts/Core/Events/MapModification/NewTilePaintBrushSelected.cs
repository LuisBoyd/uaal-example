using RCRCoreLib.Core.Systems.Tiles;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.MapModification
{
    public class NewTilePaintBrushSelected : GameEvent
    {
        public TileBase tile;
        public TileSelectionOptions option;

        public NewTilePaintBrushSelected(TileBase tile, TileSelectionOptions option)
        {
            this.tile = tile;
            this.option = option;
        }
    }
}