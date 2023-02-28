namespace RCRCoreLib.TilePaintingSystem
{
    public class PainterActiveStateSwitchEvent : GameEvent
    {
        public bool State;

        public PainterActiveStateSwitchEvent(bool active)
        {
            State = active;
        }
    }
}