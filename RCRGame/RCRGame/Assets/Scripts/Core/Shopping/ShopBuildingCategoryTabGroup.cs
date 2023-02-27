using RCRCoreLib.Core.Shopping.Category;
using RCRCoreLib.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Shopping
{
    public class ShopBuildingCategoryTabGroup : TabGroup
    {
        [SerializeField] 
        private BuildingCategory Category;

        public override void OnPointerDown(PointerEventData eventData)
        {
            var BuildingCategorySelectedEvent = new RefreshShopBuildingUI(Category);
            EventManager.Instance.QueueEvent(BuildingCategorySelectedEvent);
        }

        public override void OnPointerUp(PointerEventData eventData){}
        
    }
}