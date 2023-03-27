namespace RCRCoreLib.Core.Events.XPLevel
{
    public class XPAddedGameEvent : GameEvent
    {
        public int amount;

        public XPAddedGameEvent(int amount)
        {
            this.amount = amount;
        }
    }
}