using RCRCoreLib.Core.Systems;

namespace RCRCoreLib.Core.Events.System
{
    public class SystemActivateEvent : GameEvent
    {
        public bool Active;
        public SystemType type;

        public SystemActivateEvent(bool active, SystemType type)
        {
            this.type = type;
            this.Active = active;
        }
    }
}