using CustomUIFramework.Animation.AnimationProperties;
using UnityEngine;
using UnityEngine.UI;

namespace CustomUIFramework.Animation.Behaviors
{
    public abstract class FrameworkStateMachineBehaviour <T,U> : StateMachineBehaviour
    where T : IAnimationProperty where U : Selectable
    {
        [SerializeField] 
        protected T _animationProperty;
        [SerializeField] 
        protected U _selectable;

        public void SetSelectable(U selectable) => _selectable = selectable;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_animationProperty == null || _selectable == null)
                return;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_animationProperty == null || _selectable == null)
                return;
        }
    }
}