using System.Collections;
using RCRCoreLib.Core.CameraLib;
using RCRCoreLib.Core.Systems.Tutorial;
using RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates;
using UnityEngine;

namespace RCRCoreLib.Core.Node.Nodes
{
    public class TutorialPan : TutorialNodeAction<TutorialWaitForCameraPan>
    {
        public float Limit;

        public override void Execute()
        {
            base.Execute();
            action = new TutorialWaitForCameraPan(Limit);
            PanZoom.Instance.MoveEnabled = true;
            TutorialGuide.Instance.StartCoroutine(WaitFor());
        }

        protected override IEnumerator WaitFor()
        {
            yield return action;
            PanZoom.Instance.MoveEnabled = false;
            NextNode("exit");
        }
    }
}