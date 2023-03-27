using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Shopping.Category;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Shopping
{
    public class ShopDecorationCategoryTabGroup : TabGroup
    {
        [SerializeField] 
        private DecorationCategory Category;

        protected  void OnEnable()
        {
            
            EventManager.Instance.AddListener<RefreshShopDecorationUI>(On_BuildingBtnClicked);
        }

        protected  void OnDisable()
        {
            EventManager.Instance.RemoveListener<RefreshShopDecorationUI>(On_BuildingBtnClicked);
           
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var DecorationCategorySelectedEvent = new RefreshShopDecorationUI(Category);
            EventManager.Instance.QueueEvent(DecorationCategorySelectedEvent);
        }

        public override void OnPointerUp(PointerEventData eventData){}

        private void On_BuildingBtnClicked(RefreshShopDecorationUI evnt)
        {
            if(evnt.category == Category)
                Open();
            else
            {
                Close();
            }
        }

        public override void Open()
        {
          
        }

        public override void Close()
        {
           
        }
    }
}