using System;
using UnityEngine;

namespace RCRCoreLib.Core.Utilities
{
    public static class Extenstions
    {
        public static LTDescr ScaleGUI(RectTransform transform, Vector2 to, float time)
        {
            Action<Vector2> TweenAction = (Vector2 v) =>
            {
                transform.sizeDelta = v;
            };
            LTDescr ltDescr = LeanTween.value(transform.gameObject,
                transform.sizeDelta, to, time);
            ltDescr.setOnUpdateVector2(TweenAction);
            //TODO maybe kill the Deleagate after compeltion
            return ltDescr;
        }
        
    }
}