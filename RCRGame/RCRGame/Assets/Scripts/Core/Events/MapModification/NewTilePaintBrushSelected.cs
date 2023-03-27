using RCRCoreLib.Core.Systems.Tiles;

namespace RCRCoreLib.Core.Events.MapModification
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