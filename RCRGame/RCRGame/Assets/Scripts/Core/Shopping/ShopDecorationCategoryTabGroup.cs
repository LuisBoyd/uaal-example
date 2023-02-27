using RCRCoreLib.Core.Shopping.Category;
using RCRCoreLib.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Shopping
{
    public class ShopDecorationCategoryTabGroup : TabGroup
    {
        [SerializeField] 
        private DecorationCategory Category;

        public override void OnPointerDown(PointerEventData eventData)
        {
            var DecorationCategorySelectedEvent = new RefreshShopDecorationUI(Category);
            EventManager.Instance.QueueEvent(DecorationCategorySelectedEvent);
        }

        public override void OnPointerUp(PointerEventData eventData){}
       
    }
}