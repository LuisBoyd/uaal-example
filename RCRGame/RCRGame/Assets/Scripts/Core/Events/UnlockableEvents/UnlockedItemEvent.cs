namespace RCRCoreLib.UnlockableEvents
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