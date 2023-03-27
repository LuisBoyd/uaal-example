using RCRCoreLib.Core.UI.UISystem;

namespace RCRCoreLib.Core.Events.UI
{
    public class ActivateUIEvent : GameEvent
    {
        public bool active;
        public UIType uiType;

        public ActivateUIEvent(bool active, UIType type)
        {
            this.active = active;
            this.uiType = type;
        }
    }
}