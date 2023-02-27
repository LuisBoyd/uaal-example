using System;
using UnityEngine;

namespace RCRCoreLib.Core.UI.Animation
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class AnimatedUI : MonoBehaviour, IAnimatedUI
    {
        [Header("In Methods")]
        [SerializeField] protected bool slideIn;
        [SerializeField] protected bool fadeIn;
        [SerializeField] protected bool scaleIn;
        
        [Header("Out Methods")]
        [SerializeField] protected bool slideOut;
        [SerializeField] protected bool fadeOut;
        [SerializeField] protected bool scaleOut;

        [Header("Timing")] 
        [SerializeField] protected float TimeToLerp;

        protected RectTransform UItransform;

        protected void Awake()
        {
            UItransform = GetComponent<RectTransform>();
        }

        protected virtual void OnEnable()
        {
            if (slideIn)
                SlideIn();
            else if (fadeIn)
                FadeIn();
            else if(scaleIn)
                ScaleIn();
        }

        protected virtual void OnDisable()
        {
            if (slideOut)
                SlideOut();
            else if (fadeOut)
               FadeOut();
            else if(scaleOut)
                ScaleOut();
        }


        public abstract void SlideIn();


        public abstract void SlideOut();


        public abstract void FadeIn();
 
        public abstract void FadeOut();
       

        public abstract void ScaleIn();
       

        public abstract void ScaleOut();
    }
}