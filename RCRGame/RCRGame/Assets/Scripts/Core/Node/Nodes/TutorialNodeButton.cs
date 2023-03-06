using System.Collections;
using RCRCoreLib.Core.Systems.Tutorial;
using RCRCoreLib.Core.Systems.Tutorial.Enum;
using RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Node.Nodes
{
    public class TutorialNodeButton : TutorialNodeAction<TutorialWaitForButtonClick>
    {
        public TutorialBtnType type; //Interacts with the Tutorial System gets the btn the user should click.
        public Vector2 FingerPos;
        public float FingerRotation;
        public float Topos;
        public bool FingerIsHorizontal;
        private Button btnToClick;
        private RectTransform btnToClickTransform;
        private LTDescr fingerAnimation;
        
        public override void Execute()
        {
            base.Execute();
            btnToClick = TutorialGuide.Instance.TutBtnDictionary[type];
            btnToClickTransform = btnToClick.GetComponent<RectTransform>();
            btnToClick.interactable = true;
            fingerAnimation = TutorialGuide.Instance.EnableUnmask(btnToClickTransform, FingerPos,FingerRotation,Topos, FingerIsHorizontal);
            //Should throw an error if the type is not in there.
            action = new TutorialWaitForButtonClick(btnToClick);
            TutorialGuide.Instance.StartCoroutine(WaitFor());
        }

        public override void NextNode(string _exit)
        {
            LeanTween.cancel(fingerAnimation.id);
            btnToClick.interactable = false;
            base.NextNode(_exit);
        }

        protected override IEnumerator WaitFor()
        {
            yield return action;
            NextNode("exit");
        }
    }
}