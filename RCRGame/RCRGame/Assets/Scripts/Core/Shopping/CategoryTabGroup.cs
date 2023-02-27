using System;
using RCRCoreLib.Core.Shopping.Category;
using RCRCoreLib.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Shopping
{
    public class CategoryTabGroup : TabGroup
    {
        [SerializeField] 
        private ShoppingTabGroup Category;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<ShoppingTabGroupClicked>(OnSelected);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<ShoppingTabGroupClicked>(OnSelected);
        }

        public override void SlideIn()
        {
            throw new NotImplementedException();
        }

        public override void SlideOut()
        {
            throw new NotImplementedException();
        }

        public override void FadeIn()
        {
            throw new NotImplementedException();
        }

        public override void FadeOut()
        {
            throw new NotImplementedException();
        }

        public override void ScaleIn()
        {
            throw new NotImplementedException();
        }

        public override void ScaleOut()
        {
            throw new NotImplementedException();
        }

        public override void Selected()
        {
            Debug.Log($"Selected {Category.ToString()}");
        }

        public override void Unselected()
        {
            throw new NotImplementedException();
        }

        private void OnSelected(ShoppingTabGroupClicked evnt)
        {
            if(evnt.group == Category)
                Selected();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var CategoryClickedEvent = new ShoppingTabGroupClicked(Category);
            var ClearCardUIEvent = new ClearCardViewUI();
            EventManager.Instance.QueueEvent(CategoryClickedEvent);
            EventManager.Instance.QueueEvent(ClearCardUIEvent);
        }

        public override void OnPointerUp(PointerEventData eventData){}
        
    }
}