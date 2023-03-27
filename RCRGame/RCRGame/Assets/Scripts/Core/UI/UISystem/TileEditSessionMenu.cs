using System;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.MapModification;
using RCRCoreLib.Core.Events.System;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI.UISystem
{
    public class TileEditSessionMenu : UIMenuRoot
    {
        [SerializeField] 
        private TextMeshProUGUI desrciption;
        [SerializeField] 
        private Button closeBtn;
        [SerializeField] 
        private Button confirmBtn;

        private void OnEnable()
        {
            closeBtn.onClick.AddListener(DenySession);
            confirmBtn.onClick.AddListener(ConfirmSession);
        }

        private void OnDisable()
        {
            closeBtn.onClick.RemoveListener(DenySession);
            confirmBtn.onClick.RemoveListener(ConfirmSession);
        }

        public override void Open()
        {
            
        }

        public override void Close()
        {
            
        }

        public override void Activate()
        {
            confirmBtn.gameObject.SetActive(true);
            closeBtn.gameObject.SetActive(true);
            desrciption.gameObject.SetActive(true);
            this.gameObject.SetActive(true);
        }

        public override void DeActivate()
        {
           confirmBtn.gameObject.SetActive(false);
           closeBtn.gameObject.SetActive(false);
           desrciption.gameObject.SetActive(false);
           this.gameObject.SetActive(false);
           EventManager.Instance.QueueEvent(new SystemActivateEvent(false, SystemType.TileEditSystem)); //Deactivate system when everything is done
        }

        private void ConfirmSession()
        {
            EventManager.Instance.QueueEvent(new ActivateUIEvent(false, UIType.TileEditSessionMenu));
            EventManager.Instance.QueueEvent(new ActivateUIEvent(true, UIType.HUDBottom));
            EventManager.Instance.QueueEvent(new TileSessionEndEvent(true));
        }

        private void DenySession()
        {
            EventManager.Instance.QueueEvent(new ActivateUIEvent(false, UIType.TileEditSessionMenu));
            EventManager.Instance.QueueEvent(new ActivateUIEvent(true, UIType.HUDBottom));
            EventManager.Instance.QueueEvent(new TileSessionEndEvent(false));
        }
        
    }
}