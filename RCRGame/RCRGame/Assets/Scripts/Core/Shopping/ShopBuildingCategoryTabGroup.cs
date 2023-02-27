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

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.Instance.AddListener<RefreshShopBuildingUI>(On_BuildingBtnClicked);
        }
        protected override void OnDisable()
        {
            EventManager.Instance.RemoveListener<RefreshShopBuildingUI>(On_BuildingBtnClicked);
            base.OnDisable();
        }

        private void On_BuildingBtnClicked(RefreshShopBuildingUI evnt)
        {
            if(evnt.category == Category)
                Selected();
            else
            {
                Unselected();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var BuildingCategorySelectedEvent = new RefreshShopBuildingUI(Category);
            EventManager.Instance.QueueEvent(BuildingCategorySelectedEvent);
        }

        public override void OnPointerUp(PointerEventData eventData){}

        public override void SlideIn()
        {
            throw new System.NotImplementedException();
        }

        public override void SlideOut()
        {
            throw new System.NotImplementedException();
        }

        public override void FadeIn()
        {
            throw new System.NotImplementedException();
        }

        public override void FadeOut()
        {
            throw new System.NotImplementedException();
        }

        public override void ScaleIn()
        {
            throw new System.NotImplementedException();
        }

        public override void ScaleOut()
        {
            throw new System.NotImplementedException();
        }

        public override void Selected()
        {
           
        }

        public override void Unselected()
        {
           
        }
    }
}