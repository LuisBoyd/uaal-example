using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.MapModification;
using RCRCoreLib.Core.Events.UI;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Core.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Systems.Tiles
{
    public class TileGroupTabGroup : TileSelectionTab
    {
        [SerializeField] 
        private RectTransform TabGroup;

        [SerializeField] 
        private RectTransform Indicator;

        [SerializeField] 
        private List<SubTileSelectionTab> UIIgnoreTypes
             = new List<SubTileSelectionTab>();

        private int TapCount = 0;

        private void Start()
        {
            foreach (SubTileSelectionTab tileSelectionTab in UIIgnoreTypes)
            {
                tileSelectionTab.OnPressed += SubStates;
            }
        }

        private void SubStates(object sender, SubTileSelectionTab e)
        {
            foreach (SubTileSelectionTab subTileSelectionTab in UIIgnoreTypes)
            {
                subTileSelectionTab.Close();
            }
            e.Open();
        }

        // protected override void OnNewTilePaintBrushSelected(NewTilePaintBrushSelected evnt)
        // {
        //     if (evnt.option == option)
        //     {
        //         Open();
        //     }
        //     else
        //     {
        //         if (!UIIgnoreTypes.Contains(evnt.option))
        //         {
        //             Close();
        //             CurrentlySelected = false;
        //         }
        //     }
        // }
        //
        // public override void Open()
        // {
        //     base.Open();
        //     Indicator.gameObject.SetActive(true);
        //     if (!CurrentlySelected)
        //     {
        //         TabGroup.gameObject.SetActive(false);
        //         CurrentlySelected = true;
        //     }
        //     else
        //     {
        //         TabGroup.gameObject.SetActive(true);
        //         CurrentlySelected = false;
        //     }
        // }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (TapCount == 0)
            {
                EventManager.Instance.QueueEvent(new GroupTabSelectedEvent(this));
            }
            else
            {
                Open();
            }
        }

        public override void Open()
        {
            TapCount++;
            if (TapCount > 2)
            {
                CloseSubMenu();
                return;
            }
            Debug.Log($"This is the Tap Count {TapCount.ToString()}");
            switch (TapCount)
            {
                case 1: //HighLight Button, ShowIndication
                    Extenstions.ScaleGUI(selfTransform, SelectedSizeScale, time_to_lerp);
                    Buttonbackground.color = SelectedColor;
                    Indicator.gameObject.SetActive(true);
                    break;
                case 2: //Reveal SubTile's
                    TabGroup.gameObject.SetActive(true);
                    break;
            }
            EventManager.Instance.QueueEvent(new NewTilePaintBrushSelected(option));
        }

        public override void Close()
        {
            CloseSubMenu();
        }


        private void CloseSubMenu()
        {
            TapCount = 0;
            Extenstions.ScaleGUI(selfTransform, UnSelectedSizeScale, time_to_lerp);
            Buttonbackground.color = UnSelectedColor;
            Indicator.gameObject.SetActive(false);
            TabGroup.gameObject.SetActive(false);
        }
        
    }
}