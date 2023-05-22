using CustomUIFramework.Organisms;
using UnityEngine;

namespace CustomUIFramework.Technical.Transition
{
    [CreateAssetMenu(fileName = "new_InstantTransitionConfig", 
        menuName = "RCR/Animation/Navigation/Transition/Instant Transition Config", order = 0)]
    public class InstantTransitionConfig : TransitionConfig
    {
        public override void ShowTransition(SlicePanel slicePanel)
        {
            StartTransition();
            slicePanel.gameObject.SetActive(true);
            EndTransition();
        }

        public override void HideTransition(SlicePanel slicePanel)
        {
            StartTransition();
            slicePanel.gameObject.SetActive(false);
            EndTransition();
        }
    }
}