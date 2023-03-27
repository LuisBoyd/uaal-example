using System;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.System;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI.UISystem
{
    public class ConstructionMenu : UIMenuRoot
    {
        [SerializeField] 
        private UIAnimated HorizontalStructureSelection;
        [SerializeField] 
        private UIAnimated StructureCategorySelection;
        [SerializeField] 
        private Button CloseBtn;

        private void OnEnable()
        {
            CloseBtn.onClick.AddListener(CloseUI);
        }

        private void OnDisable()
        {
            CloseBtn.onClick.RemoveListener(CloseUI);
        }

        public override void Open()
        {
            HorizontalStructureSelection.Open();
            StructureCategorySelection.Open();
        }

        public override void Close()
        {
            HorizontalStructureSelection.Close();
            StructureCategorySelection.Close();
        }

        public override void Activate()
        {
            this.gameObject.SetActive(true);
            HorizontalStructureSelection.gameObject.SetActive(true);
            StructureCategorySelection.gameObject.SetActive(true);
            CloseBtn.gameObject.SetActive(true);
            Open();
            EventManager.Instance.QueueEvent(new SystemActivateEvent(true, SystemType.ShoppingSystem));
        }

        public override void DeActivate()
        {
           Close();
           HorizontalStructureSelection.gameObject.SetActive(false);
           StructureCategorySelection.gameObject.SetActive(false);
           CloseBtn.gameObject.SetActive(false);
           this.gameObject.SetActive(false);
        }

        private void CloseUI()
        {
            var CleanupCardUI = new ClearCardViewUI();
            var evntCloseShop = new ActivateUIEvent(false, UIType.StructureBuildingSelection);
            var evntOpenHUD = new ActivateUIEvent(true, UIType.HUDBottom);
            EventManager.Instance.QueueEvent(CleanupCardUI);
            EventManager.Instance.QueueEvent(new SystemActivateEvent(false, SystemType.ShoppingSystem));
            EventManager.Instance.QueueEvent(evntCloseShop);
            EventManager.Instance.QueueEvent(evntOpenHUD);
        }
        
    }
}