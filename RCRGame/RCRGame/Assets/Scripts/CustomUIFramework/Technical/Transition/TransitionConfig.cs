using Core3.SciptableObjects;
using CustomUIFramework.Organisms;
using UnityEngine;

namespace CustomUIFramework.Technical.Transition
{
    public abstract class TransitionConfig : GenericBaseScriptableObject<TransitionConfig>
    {
        public delegate void TransitionStart();
        public delegate void TransitionEnd();

        public event TransitionStart TransitionStartEvent;
        public event TransitionEnd TransitionEndEvent;

        [SerializeField] 
        protected float transitionTime;
        
        public abstract void ShowTransition(SlicePanel slicePanel);
        public abstract void HideTransition(SlicePanel slicePanel);

        protected void StartTransition() => TransitionStartEvent?.Invoke();
        protected void EndTransition() => TransitionEndEvent?.Invoke();
    }
}