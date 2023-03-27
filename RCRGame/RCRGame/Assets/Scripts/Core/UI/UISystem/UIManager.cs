using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Core.Tiles;
using RCRCoreLib.Core.Utilities.SerializableDictionary;
using UnityEngine;

namespace RCRCoreLib.Core.UI.UISystem
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : Singelton<UIManager>
    {
        [SerializeField] private UIrectDictionary m_uIrectDictionary = null;
        public IDictionary<UIType, UIMenuRoot> WorldTileDictionary
        {
            get { return m_uIrectDictionary; }
            set{m_uIrectDictionary.CopyFrom(value);}
        }

        private Canvas m_mainCanvas;
        public Canvas MainCanvas
        {
            get => m_mainCanvas;
        }

        public List<TabGroup> tabGroups = new List<TabGroup>();

        protected override void Awake()
        {
            base.Awake();
            m_mainCanvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            EventManager.Instance.AddListener<ActivateUIEvent>(On_ChangeState);
            EventManager.Instance.AddListener<GroupTabSelectedEvent>(On_TabGroup_Selected);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<ActivateUIEvent>(On_ChangeState);
        }

        private void On_ChangeState(ActivateUIEvent evnt)
        {
            UIMenuRoot instance = WorldTileDictionary[evnt.uiType];
            if (instance == null)
            {
                Debug.LogError($"{evnt.uiType.ToString()} is not in collection");
                return;
            }
            if (evnt.active)
            {
                instance.Activate();
            }
            else
            {
                instance.DeActivate();
            }
        }

        private void On_TabGroup_Selected(GroupTabSelectedEvent e)
        {
            foreach (TabGroup tabGroup in tabGroups)
            {
                if(!tabGroup.gameObject.activeSelf)
                    continue;
                tabGroup.Close();
            }
            e.tabGroup.Open();
        }
    }
}