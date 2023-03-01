namespace RCRCoreLib.Core.Events.Input
{
    public class SetInputState : GameEvent
    {
        public bool state;

        public SetInputState(bool state)
        {
            this.state = state;
        }
    }
}