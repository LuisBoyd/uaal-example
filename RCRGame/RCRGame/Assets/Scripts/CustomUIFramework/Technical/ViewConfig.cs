using System.Collections.Generic;
using Core3.SciptableObjects;
using CustomUIFramework.Organisms;
using CustomUIFramework.Technical.Transition;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomUIFramework.Technical
{
    [CreateAssetMenu(fileName = "new_ViewConfig", 
        menuName = "RCR/Animation/View Config", order = 0)]
    public class ViewConfig : GenericBaseScriptableObject<ViewConfig>
    {
        [BoxGroup("View Config")]
        [SerializeField]
        private bool _HidePrevious;
        public bool HidePrevious
        {
            get => _HidePrevious;
        }
        [BoxGroup("View Config")]
        [SerializeField]
        private bool _BlockInteractionDuringTransition;
        public bool BlockInteractionDuringTransition
        {
            get => _BlockInteractionDuringTransition;
        }
        [BoxGroup("View Config")]
        [SerializeField]
        private TransitionConfig showTransitionConfig;
        public TransitionConfig ShowTransitionConfig
        {
            get => showTransitionConfig;
        }
        [BoxGroup("View Config")]
        [SerializeField]
        private TransitionConfig hideTransitionConfig;
        public TransitionConfig HideTransitionConfig
        {
            get => hideTransitionConfig;
        }

        [SerializeField] 
        public List<SliceConfig> SliceConfigList;
    }
}