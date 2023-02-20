using System;
using RCRCoreLib.Core.Timers;
using UnityEngine;

namespace RCRCoreLib.Core.Building
{
    public class Building : DelayablePlaceableObject
    {
        
        private void OnMouseUpAsButton()
        {
            TimerToolTip.Instance.ShowTimer_Static(gameObject);
        }
    }
}