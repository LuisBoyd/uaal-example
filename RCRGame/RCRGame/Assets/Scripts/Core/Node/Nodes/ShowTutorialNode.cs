using RCRCoreLib.Core.Systems.Tutorial;
using RCRCoreLib.TutorialEvents;
using UnityEngine;
using XNode;

namespace RCRCoreLib.Core.Node.Nodes
{
    public class ShowTutorialNode : BaseTutorialNode
    {
        public override object GetValue(NodePort port)
        {
            return base.GetValue(port);
        }

        protected override void Init()
        {
            base.Init();
        }

        public override void Execute()
        {
            var showUIEvent = new ShowTutorialInterface(Message, LocationOnScreen, HorizontalFlipped);
            EventManager.Instance.QueueEvent(showUIEvent);
            Debug.Log(Message);
            TutorialGuide.Instance.ContinueButton.onClick.AddListener(Continue);
            TutorialGuide.Instance.SkipButton.onClick.AddListener(Skip);
        }
        

        private void Continue()
        {
            TutorialGuide.Instance.ContinueButton.onClick.RemoveListener(Continue);
            TutorialGuide.Instance.ContinueButton.onClick.RemoveListener(Skip);
            EventManager.Instance.QueueEvent(new HideTutorialinterface());
            NextNode("exit");
        }
        private void Skip()
        {
            TutorialGuide.Instance.ContinueButton.onClick.RemoveListener(Continue);
            TutorialGuide.Instance.ContinueButton.onClick.RemoveListener(Skip);
            EventManager.Instance.QueueEvent(new HideTutorialinterface());
            EventManager.Instance.QueueEvent(new EndTutorialEvent());
        }
    }
}