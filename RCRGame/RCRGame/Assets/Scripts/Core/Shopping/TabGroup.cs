using System;
using System.Collections.Generic;
using RCRCoreLib.Core.UI.UISystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Shopping
{
    public abstract class TabGroup: UIAnimated, IPointerDownHandler, IPointerUpHandler
    {
        public abstract void OnPointerDown(PointerEventData eventData);
        public abstract void OnPointerUp(PointerEventData eventData);

    }
}