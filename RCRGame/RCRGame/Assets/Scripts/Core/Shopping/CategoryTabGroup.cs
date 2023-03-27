using System;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Shopping.Category;
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
            //EventManager.Instance.AddListener<ShoppingTabGroupClicked>(OnSelected);
        }

        private void OnDisable()
        {
            //EventManager.Instance.RemoveListener<ShoppingTabGroupClicked>(OnSelected);
        }
        private void OnSelected(ShoppingTabGroupClicked evnt)
        {
            // if(evnt.group == Category)
            //     Selected();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var CategoryClickedEvent = new ShoppingTabGroupClicked(Category);
            var ClearCardUIEvent = new ClearCardViewUI();
            EventManager.Instance.QueueEvent(CategoryClickedEvent);
            EventManager.Instance.QueueEvent(ClearCardUIEvent);
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