using System;
using CustomUIFramework.Animation;
using CustomUIFramework.Technical.Transition;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomUIFramework.Organisms
{
    [RequireComponent(typeof(RectTransform))]
    public class SlicePanel : MonoBehaviour
    {
        [SerializeField] 
        private TransitionConfig transitionIn;
        [SerializeField] 
        private TransitionConfig transitionOut;
        
        public TransitionConfig TransitionIn
        {
            get => transitionIn;
        }
        public TransitionConfig TransitionOut
        {
            get => transitionOut;
        }
    }
}