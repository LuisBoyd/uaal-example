using System;
using UnityEngine;

namespace RCRCoreLib.Core.UI.Animation
{
    public class BuildingCategoryUI : AnimatedUI
    {

        [SerializeField] private Vector2 DisabledPosition;
        [SerializeField] private Vector2 EnabledPosition;

        protected override void OnEnable()
        {
            UItransform.localPosition = new Vector3(DisabledPosition.x, DisabledPosition.y, 0);
            base.OnEnable();
        }

        public override void SlideIn()
        {
            LeanTween.moveY(UItransform, EnabledPosition.y, TimeToLerp)
                .setEase(LeanTweenType.easeInOutBack);
        }

        public override void SlideOut()
        {
        }

        public override void FadeIn()
        {
            throw new System.NotImplementedException();
        }

        public override void FadeOut()
        {
            throw new System.NotImplementedException();
        }

        public override void ScaleIn()
        {
            throw new System.NotImplementedException();
        }

        public override void ScaleOut()
        {
            throw new System.NotImplementedException();
        }
    }
}