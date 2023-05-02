using System;
using UnityEngine;

namespace UI.UIAnimation
{
    public class ScaleUpAnimation : UserAnimation
    {
        [SerializeField] private Vector3 maxscalesize;
        [SerializeField] private Vector3 minscalesize;
        public override void Animate(Transform target, Action callWhenFinished)
        {
            base.Animate(target, callWhenFinished);
            _tween = LeanTween.scale(_rectTransform, maxscalesize,
                duration);
            if (callWhenFinished != null)
                _tween.setOnComplete(callWhenFinished);
        }

        public override void ReverseAnimate(Transform target, Action callWhenFinished)
        {
            LeanTween.cancel(_tween.id);
            base.ReverseAnimate(target, callWhenFinished);
            _tween = LeanTween.scale(_rectTransform, minscalesize,
                duration);
            if (callWhenFinished != null)
                _tween.setOnComplete(callWhenFinished);
        }
    }
}