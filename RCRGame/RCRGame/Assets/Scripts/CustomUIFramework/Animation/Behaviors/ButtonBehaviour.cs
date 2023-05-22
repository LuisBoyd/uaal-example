using System.Collections.Generic;
using System.Drawing;
using CustomUIFramework.Animation.AnimationProperties;
using CustomUIFramework.atoms;
using Sirenix.OdinInspector;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

namespace CustomUIFramework.Animation.Behaviors
{
    [CreateAssetMenu(fileName = "new_ButtonBehavior", 
        menuName = "RCR/Animation/States/Button behavior", order = 0)]
    public class ButtonBehaviour : FrameworkStateMachineBehaviour<ButtonAnimationProperty, ButtonAtom>
    {
        private LTDescr colorTweener;
        private LTDescr SizeTweener;
        private LTDescr PositionTweener;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            //apply transformations to the button.
            if (_animationProperty.TimeScale <= 0f)
            {
                OnQuickStateEnter();
                return;
            }
            Image buttonImage = _selectable.image;
            colorTweener = LeanTween.value(_selectable.gameObject, ChangeColor, buttonImage.color,
                new Color(_animationProperty.Color.a,
                    _animationProperty.Color.g, _animationProperty.Color.b,
                    _animationProperty.Alpha), _animationProperty.TimeScale);
            SizeTweener = LeanTween.value(_selectable.gameObject, _selectable.gameObject.transform.localScale,
                _animationProperty.Scale, _animationProperty.TimeScale).setEase(_animationProperty.EaseType);;
            PositionTweener = LeanTween.value(_selectable.gameObject, _selectable.gameObject.transform.localPosition,
                _animationProperty.Position, _animationProperty.TimeScale).setEase(_animationProperty.EaseType);
            buttonImage.sprite = _animationProperty.Sprite;
        }

        private void OnQuickStateEnter()
        {
            Image buttonImage = _selectable.image;
            buttonImage.color = new Color(_animationProperty.Color.a,
                _animationProperty.Color.g, _animationProperty.Color.b,
                _animationProperty.Alpha);
            buttonImage.sprite = _animationProperty.Sprite;
            _selectable.gameObject.transform.position = _animationProperty.Position;
            _selectable.gameObject.transform.localScale = _animationProperty.Scale;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            LeanTween.cancel(colorTweener.id);
            LeanTween.cancel(SizeTweener.id);
            LeanTween.cancel(PositionTweener.id);
        }

        // private void ChangePosition(Vector3 newPosition)
        // {
        //     
        // }
        // private void ChangeSize(Vector3 size)
        // {
        //     
        // }
        private void ChangeColor(Color color)
        {
            _selectable.image.color = color;
        }
    }
}