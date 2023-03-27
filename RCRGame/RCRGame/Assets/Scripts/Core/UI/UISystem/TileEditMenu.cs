using System;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.System;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI.UISystem
{
    public class TileEditMenu : UIMenuRoot
    {
        [SerializeField] 
        private UIAnimated TileMenuBar;
        [SerializeField] 
        private Button CloseTileEditMenuBtn;

        private void OnEnable()
        {
           CloseTileEditMenuBtn.onClick.AddListener(CloseTileEditMenu);
        }

        private void OnDisable()
        {
            CloseTileEditMenuBtn.onClick.RemoveListener(CloseTileEditMenu);
        }

        public override void Open()
        {
            TileMenuBar.Open();
        }

        public override void Close()
        {
           TileMenuBar.Close();
        }

        public override void Activate()
        {
            CloseTileEditMenuBtn.gameObject.SetActive(true);
            TileMenuBar.gameObject.SetActive(true);
            this.gameObject.SetActive(true);
            Open();
            EventManager.Instance.QueueEvent(new SystemActivateEvent(true, SystemType.TileEditSystem));
        }

        public override void DeActivate()
        { 
            Close();
            CloseTileEditMenuBtn.gameObject.SetActive(false);
            TileMenuBar.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void CloseTileEditMenu()
        {
            var CloseEvnt = new ActivateUIEvent(false, UIType.TileEditMenu);
            var OpenHud = new ActivateUIEvent(true, UIType.TileEditSessionMenu);
            EventManager.Instance.QueueEvent(CloseEvnt);
            EventManager.Instance.QueueEvent(OpenHud);
            
        }
        
    }
}