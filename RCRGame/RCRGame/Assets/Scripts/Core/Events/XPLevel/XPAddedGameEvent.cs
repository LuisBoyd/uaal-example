namespace RCRCoreLib.XPLevel
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