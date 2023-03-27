using RCRCoreLib.Core.Shopping.Category;

namespace RCRCoreLib.Core.Events.UI
{
    public class RefreshShopDecorationUI : GameEvent
    {
        public DecorationCategory category;

        public RefreshShopDecorationUI(DecorationCategory category)
        {
            this.category = category;
        }
    }
}