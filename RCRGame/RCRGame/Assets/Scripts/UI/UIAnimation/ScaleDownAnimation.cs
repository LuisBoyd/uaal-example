using System;
using UnityEngine;

namespace UI.UIAnimation
{
    public class ScaleDownAnimation : UserAnimation
    {
        [SerializeField] private Vector3 maxscalesize;

        public override void Animate(Transform target, Action callWhenFinished)
        {
            base.Animate(target, callWhenFinished);
            _tween = LeanTween.scale(_rectTransform, maxscalesize,
                duration);
            if (callWhenFinished != null)
                _tween.setOnComplete(callWhenFinished);
        }
    }
}