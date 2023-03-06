using System.Collections;
using RCRCoreLib.Core.CameraLib;
using RCRCoreLib.Core.Systems.Tutorial;
using RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates;

namespace RCRCoreLib.Core.Node.Nodes
{
    public class TutorialZoom : TutorialNodeAction<TutorialWaitForCameraZoom>
    {
        public float Limit;
        
        public override void Execute()
        {
            base.Execute();
            action = new TutorialWaitForCameraZoom(Limit);
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