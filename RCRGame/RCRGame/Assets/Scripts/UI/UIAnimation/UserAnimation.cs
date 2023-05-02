using System;
using deVoid.UIFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIAnimation
{
    public class UserAnimation : ATransitionComponent
    {
        [Title("Animation Configuration")]
        [SerializeField] protected float duration = 0.5f;

        protected RectTransform _rectTransform;
        protected LTDescr _tween;
        
        public override void Animate(Transform target, Action callWhenFinished)
        {
            if (_rectTransform == null)
                _rectTransform = target.GetComponent<RectTransform>();
        }

        public virtual void ReverseAnimate(Transform target, Action callWhenFinished)
        {
            if (_rectTransform == null)
                _rectTransform = target.GetComponent<RectTransform>();
        }

        public virtual void CancelAnimation()
        {
            if(_tween != null)
                LeanTween.cancel(_tween.id);
        }
    }
}