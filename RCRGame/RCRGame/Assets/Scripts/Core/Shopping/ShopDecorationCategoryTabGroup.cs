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

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.Instance.AddListener<RefreshShopDecorationUI>(On_BuildingBtnClicked);
        }

        protected override void OnDisable()
        {
            EventManager.Instance.RemoveListener<RefreshShopDecorationUI>(On_BuildingBtnClicked);
            base.OnDisable();
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
                Selected();
            else
            {
                Unselected();
            }
        } 
        
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