using RCRCoreLib.Core.Shopping.Category;

namespace RCRCoreLib.UI
{
    public class ShoppingTabGroupClicked : GameEvent
    {
        public ShoppingTabGroup group;

        public ShoppingTabGroupClicked(ShoppingTabGroup group)
        {
            this.group = group;
        }
    }
}