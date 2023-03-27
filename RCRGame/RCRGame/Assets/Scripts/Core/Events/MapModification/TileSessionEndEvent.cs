namespace RCRCoreLib.Core.Events.MapModification
{
    public class TileSessionEndEvent : GameEvent
    {
        public bool save;

        public TileSessionEndEvent(bool save)
        {
            this.save = save;
        }
    }
}