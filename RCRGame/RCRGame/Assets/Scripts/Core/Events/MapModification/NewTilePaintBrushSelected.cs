using RCRCoreLib.Core.Systems.Tiles;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.MapModification
{
    public class NewTilePaintBrushSelected : GameEvent
    {
        public TileSelectionOptions option;

        public NewTilePaintBrushSelected(TileSelectionOptions option)
        {
            this.option = option;
        }
    }
}