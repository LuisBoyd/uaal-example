using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Shopping.Category;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Shopping
{
    public class ShopBuildingCategoryTabGroup : TabGroup
    {
        [SerializeField] 
        private BuildingCategory Category;

        protected  void OnEnable()
        {
          
            EventManager.Instance.AddListener<RefreshShopBuildingUI>(On_BuildingBtnClicked);
        }
        protected  void OnDisable()
        {
            EventManager.Instance.RemoveListener<RefreshShopBuildingUI>(On_BuildingBtnClicked);
        }

        private void On_BuildingBtnClicked(RefreshShopBuildingUI evnt)
        {
            if(evnt.category == Category)
                Open();
            else
            {
                Close();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var BuildingCategorySelectedEvent = new RefreshShopBuildingUI(Category);
            EventManager.Instance.QueueEvent(BuildingCategorySelectedEvent);
        }

        public override void OnPointerUp(PointerEventData eventData){}

  

        public override void Open()
        {
          
        }

        public override void Close()
        {
           
        }
    }
}