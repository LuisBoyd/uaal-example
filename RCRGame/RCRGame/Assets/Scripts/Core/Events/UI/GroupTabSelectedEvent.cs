using RCRCoreLib.Core.Shopping;

namespace RCRCoreLib.Core.Events.UI
{
    public class GroupTabSelectedEvent: GameEvent
    {
        public TabGroup tabGroup;

        public GroupTabSelectedEvent(TabGroup tabGroup)
        {
            this.tabGroup = tabGroup;
        }
    }
}