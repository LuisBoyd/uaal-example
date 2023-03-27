using System;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI.UISystem
{
    public class ConstructionBtnHUD : UIMenuRoot
    {
        [SerializeField] 
        private Button TileConstructionBtn;
        [SerializeField] 
        private Button BuildingConstructionBtn;

        private void OnEnable()
        {
            BuildingConstructionBtn.onClick.AddListener(OpenShop);
            TileConstructionBtn.onClick.AddListener(OpenTileEdit);
        }

        private void OnDisable()
        {
            BuildingConstructionBtn.onClick.RemoveListener(OpenShop);
            TileConstructionBtn.onClick.RemoveListener(OpenTileEdit);
        }

        public override void Open()
        {
        }
        public override void Close()
        {
        }
        public override void Activate()
        {
            BuildingConstructionBtn.gameObject.SetActive(true);
            TileConstructionBtn.gameObject.SetActive(true);
            this.gameObject.SetActive(true);
        }
        public override void DeActivate()
        {
            BuildingConstructionBtn.gameObject.SetActive(false);
            TileConstructionBtn.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void OpenShop()
        {
            var evntOpenShop = new ActivateUIEvent(true, UIType.StructureBuildingSelection);
            var evntCloseHUD = new ActivateUIEvent(false, UIType.HUDBottom);
            EventManager.Instance.QueueEvent(evntCloseHUD);
            EventManager.Instance.QueueEvent(evntOpenShop);
        }

        private void OpenTileEdit()
        {
            var evntOpenTileMenu = new ActivateUIEvent(true, UIType.TileEditMenu);
            var evntCloseHUD = new ActivateUIEvent(false, UIType.HUDBottom);
            EventManager.Instance.QueueEvent(evntCloseHUD);
            EventManager.Instance.QueueEvent(evntOpenTileMenu);
        }
    }
}