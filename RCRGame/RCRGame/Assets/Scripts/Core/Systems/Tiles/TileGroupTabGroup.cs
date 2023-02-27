using System.Collections.Generic;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.MapModification;
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
        private List<TileSelectionOptions> UIIgnoreTypes
             = new List<TileSelectionOptions>();

        private bool CurrentlySelected = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            TabGroup.gameObject.SetActive(false);
            Indicator.gameObject.SetActive(false);
        }

        protected override void OnNewTilePaintBrushSelected(NewTilePaintBrushSelected evnt)
        {
            if (evnt.option == option)
            {
                Selected();
            }
            else
            {
                if (!UIIgnoreTypes.Contains(evnt.option))
                {
                    Unselected();
                    CurrentlySelected = false;
                }
            }
        }

        public override void Unselected()
        {
            base.Unselected();
            TabGroup.gameObject.SetActive(false);
            Indicator.gameObject.SetActive(false);
        }

        public override void Selected()
        {
            base.Selected();
            Indicator.gameObject.SetActive(true);
            if (!CurrentlySelected)
            {
                TabGroup.gameObject.SetActive(false);
                CurrentlySelected = true;
            }
            else
            {
                TabGroup.gameObject.SetActive(true);
                CurrentlySelected = false;
            }
        }
    }
}