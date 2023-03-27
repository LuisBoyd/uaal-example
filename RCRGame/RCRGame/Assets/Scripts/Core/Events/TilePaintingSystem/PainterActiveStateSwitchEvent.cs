namespace RCRCoreLib.Core.Events.TilePaintingSystem
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