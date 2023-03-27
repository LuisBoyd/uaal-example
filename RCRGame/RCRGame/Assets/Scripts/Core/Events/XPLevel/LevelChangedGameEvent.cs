namespace RCRCoreLib.Core.Events.XPLevel
{
    public class LevelChangedGameEvent : GameEvent
    {
        public int newLevel;

        public LevelChangedGameEvent(int level)
        {
            this.newLevel = level;
        }
    }
}