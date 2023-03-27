namespace RCRCoreLib.Core.Events.UnlockableEvents
{
    public class UnlockedItemEvent : GameEvent
    {
        public int UnlockedObjectID;

        public UnlockedItemEvent(int ID)
        {
            UnlockedObjectID = ID;
        }
    }
}