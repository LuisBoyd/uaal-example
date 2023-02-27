using RCRCoreLib.Core.Shopping.Category;

namespace RCRCoreLib.UI
{
    public class RefreshShopBuildingUI : GameEvent
    {
        public BuildingCategory category;

        public RefreshShopBuildingUI(BuildingCategory category)
        {
            this.category = category;
        }
    }
}