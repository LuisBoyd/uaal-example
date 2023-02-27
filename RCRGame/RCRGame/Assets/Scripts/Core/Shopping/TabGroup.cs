using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Shopping
{
    public abstract class TabGroup: MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // public Sprite tabIdle;
        // public Sprite tabActive;
        //
        // public List<TabButton> tabButtons = new List<TabButton>();
        // public List<GameObject> objectstoSwap = new List<GameObject>();
        //
        // [NonSerialized] public TabButton selectedTab;
        //
        // public void Subscribe(TabButton button)
        // {
        //     tabButtons.Add(button);
        // }
        //
        // private void Start()
        // {
        //     OnTabSelected(tabButtons[0]);
        // }
        //
        // private void ResetTabs()
        // {
        //     foreach (var button in tabButtons)
        //     {
        //         if (selectedTab != null && button == selectedTab)
        //         {
        //             continue;
        //         }
        //
        //         button.background.sprite = tabIdle;
        //     }
        // }
        //
        // public void OnTabSelected(TabButton button)
        // {
        //     selectedTab = button;
        //     ResetTabs();
        //     button.background.sprite = tabActive;
        //
        //     int index = button.transform.GetSiblingIndex();
        //     for (int i = 0; i < objectstoSwap.Count; i++)
        //     {
        //         if (i == index)
        //         {
        //             objectstoSwap[i].SetActive(true);
        //         }
        //         else
        //         {
        //             objectstoSwap[i].SetActive(false);
        //         }
        //     }
        // }
        public abstract void OnPointerDown(PointerEventData eventData);
       

        public abstract void OnPointerUp(PointerEventData eventData);

    }
}