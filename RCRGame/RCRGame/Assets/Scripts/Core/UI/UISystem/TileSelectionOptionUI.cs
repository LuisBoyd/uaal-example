using System;
using UnityEngine;

namespace RCRCoreLib.Core.UI.UISystem
{
    public class TileSelectionOptionUI : UIAnimated
    {
        [SerializeField] 
        private Vector2 DisabledPosition;
        [SerializeField] 
        private Vector2 EnabledPosition;

        private void OnEnable()
        {
            selfTransform.localPosition = new Vector3(DisabledPosition.x, DisabledPosition.y);
        }

        public override void Open()
        {
            LeanTween.moveY(selfTransform, EnabledPosition.y, time_to_lerp)
                .setEase(LeanTweenType.easeInOutBack);
        }
        public override void Close()
        {
        }
    }
}