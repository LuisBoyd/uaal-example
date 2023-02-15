using System;
using System.Collections.Generic;
using UnityEngine;

namespace RCRCoreLib.Core.Shopping
{
    public class TabGroup: MonoBehaviour
    {
        public Sprite tabIdle;
        public Sprite tabActive;

        public List<TabButton> tabButtons = new List<TabButton>();
        public List<GameObject> objectstoSwap = new List<GameObject>();

        [NonSerialized] public TabButton selectedTab;

        public void Subscribe(TabButton button)
        {
            tabButtons.Add(button);
        }

        private void Start()
        {
            OnTabSelected(tabButtons[0]);
        }

        private void ResetTabs()
        {
            foreach (var button in tabButtons)
            {
                if (selectedTab != null && button == selectedTab)
                {
                    continue;
                }

                button.background.sprite = tabIdle;
            }
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            ResetTabs();
            button.background.sprite = tabActive;

            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < objectstoSwap.Count; i++)
            {
                if (i == index)
                {
                    objectstoSwap[i].SetActive(true);
                }
                else
                {
                    objectstoSwap[i].SetActive(false);
                }
            }
        }
    }
}