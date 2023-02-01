namespace Events.Library.Models.WorldEvents
{
    public abstract class OnLogicChangedEvent : BaseEvent
    {
        public OnLogicChangedArgs Args;

        protected OnLogicChangedEvent(OnLogicChangedArgs args)
        {
            Args = args;
        }
    }
}