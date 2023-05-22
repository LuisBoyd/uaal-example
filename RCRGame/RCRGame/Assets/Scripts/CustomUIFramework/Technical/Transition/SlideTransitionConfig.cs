using System;
using CustomUIFramework.enums;
using CustomUIFramework.Organisms;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomUIFramework.Technical.Transition
{
    [CreateAssetMenu(fileName = "new_SlideTransitionConfig", 
        menuName = "RCR/Animation/Navigation/Transition/Slide Transition Config", order = 0)]
    public class SlideTransitionConfig : TransitionConfig
    {
        [BoxGroup("Show Config")]
        [SerializeField]
        private Vector2 anchoredPositionMinShow;
        [BoxGroup("Show Config")]
        [SerializeField]
        private Vector2 anchoredPositionMaxShow;
        [BoxGroup("Show Config")]
        [SerializeField]
        private Vector2 PivotShow;
        [BoxGroup("Show Config")]
        [SerializeField] 
        private LeanTweenType EaseTypeShow;
        
        [BoxGroup("Hide Config")]
        [SerializeField]
        private Vector2 anchoredPositionMinHide;
        [BoxGroup("Hide Config")]
        [SerializeField]
        private Vector2 anchoredPositionMaxHide;
        [BoxGroup("Hide Config")]
        [SerializeField]
        private Vector2 PivotHide;
        [BoxGroup("Hide Config")]
        [SerializeField] 
        private LeanTweenType EaseTypeHide;
        
        private LTDescr slideAnimation;
        
        public override void ShowTransition(SlicePanel slicePanel)
        {
            StartTransition();
            slicePanel.rectTransform.anchorMax = anchoredPositionMaxShow;
            slicePanel.rectTransform.anchorMin = anchoredPositionMinShow;
            slicePanel.rectTransform.pivot = PivotShow;
            slideAnimation = LeanTween.move(slicePanel.rectTransform,Vector3.zero, transitionTime)
                .setEase(EaseTypeShow).setOnComplete(EndTransition);
        }

        public override void HideTransition(SlicePanel slicePanel)
        {
            StartTransition();
            slicePanel.rectTransform.anchorMax = anchoredPositionMaxHide;
            slicePanel.rectTransform.anchorMin = anchoredPositionMinHide;
            slicePanel.rectTransform.pivot = PivotHide;
            slideAnimation = LeanTween.move(slicePanel.rectTransform,Vector3.zero, transitionTime)
                .setEase(EaseTypeHide).setOnComplete(EndTransition);
        }
    }
}