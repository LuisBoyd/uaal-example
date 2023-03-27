using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Utilities.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI.UISystem
{
    public class StructureCategorySelectionUI : UIMenuRoot
    {
        [SerializeField] 
        private Vector2 DisabledPosition;
        [SerializeField] 
        private Vector2 EnabledPosition;

        [SerializeField] 
        private StructureCategoryDictionary m_structureCategoryDictionary = null;
        public IDictionary<CategoryShopBtn, RectTransform> structureCategoryDictionary
        {
            get { return m_structureCategoryDictionary; }
            set {m_structureCategoryDictionary.CopyFrom(value);}
        }

        private void Start()
        {
            foreach (CategoryShopBtn button in structureCategoryDictionary.Keys)
            {
                button.onClick.AddListener(
                    () =>
                    {
                        OnBtnClick(button);
                    });
            }
        }

        private void OnEnable()
        {
            selfTransform.localPosition = new Vector3(DisabledPosition.x, DisabledPosition.y);
        }

        private void ClearCategories()
        {
            foreach (RectTransform rectTransform in structureCategoryDictionary.Values)
            {
                rectTransform.gameObject.SetActive(false);
            }
        }

        private void OnBtnClick(CategoryShopBtn btn)
        {
            ClearCategories();
            structureCategoryDictionary[btn].gameObject.SetActive(true);
            var CategoryClickedEvent = new ShoppingTabGroupClicked(btn.Category);
            var ClearCardUIEvent = new ClearCardViewUI();
            EventManager.Instance.QueueEvent(CategoryClickedEvent);
            EventManager.Instance.QueueEvent(ClearCardUIEvent);
        }
        
        public override void Open()
        {
            LeanTween.move(selfTransform, EnabledPosition, time_to_lerp)
                .setEase(LeanTweenType.easeInOutBack);
        }

        public override void Close()
        {
           
        }

        public override void Activate()
        {
           Open();
        }

        public override void DeActivate()
        {
            Close();
        }
    }
}