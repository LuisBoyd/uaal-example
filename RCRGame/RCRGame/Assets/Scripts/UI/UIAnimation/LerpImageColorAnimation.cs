using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIAnimation
{
    public class LerpImageColorAnimation : UserAnimation
    {
        [SerializeField] private Color _ColorToLerpTo;
        [SerializeField]private Color _OrignalColor;
        [SerializeField][Required] private Image _ImageToChange;


        public override void Animate(Transform target, Action callWhenFinished)
        {
            
            base.Animate(target, callWhenFinished);
            _tween = LeanTween.value(_ImageToChange.gameObject, Change_Color, _OrignalColor, _ColorToLerpTo, duration);
            if (callWhenFinished != null)
                _tween.setOnComplete(callWhenFinished);
        }

        public override void ReverseAnimate(Transform target, Action callWhenFinished)
        {
            LeanTween.cancel(_tween.id);
            base.ReverseAnimate(target, callWhenFinished);
            _tween = LeanTween.value(_ImageToChange.gameObject, Change_Color, _ColorToLerpTo, _OrignalColor, duration);
            if (callWhenFinished != null)
                _tween.setOnComplete(callWhenFinished);
            
        }

        private void Change_Color(Color c)
        {
            _ImageToChange.color = c;
        }
    }
}