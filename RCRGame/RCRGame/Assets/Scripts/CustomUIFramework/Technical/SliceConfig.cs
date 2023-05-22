using System;
using CustomUIFramework.Organisms;
using CustomUIFramework.Technical.Transition;
using Sirenix.OdinInspector;

namespace CustomUIFramework.Technical
{
    [Serializable]
    public class SliceConfig
    {
        public Slice Slice;
        public bool OverrideViewShowTransition;
        public bool OverrideViewHideTransition;

        [ShowIf("OverrideViewShowTransition")] 
        public TransitionConfig ShowTransitionConfig;
        [ShowIf("OverrideViewHideTransition")] 
        public TransitionConfig HideTransitionConfig;
    }
}