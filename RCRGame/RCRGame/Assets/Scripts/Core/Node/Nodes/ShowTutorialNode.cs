using RCRCoreLib.Core.Systems.Tutorial;
using RCRCoreLib.TutorialEvents;
using UnityEngine;
using XNode;

namespace RCRCoreLib.Core.Node.Nodes
{
    public class ShowTutorialNode : BaseTutorialNode
    {
        [TextArea]
        public string Message; //The Text that you want displayed in the text box.
        public bool HorizontalFlipped = false; // Should we reverse the order so that the avatar and text box appear in reverse order.
        public Vector2 LocationOnScreen; //Where should the UI pop up.

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

        public override void Update()
        {
            throw new System.NotImplementedException();
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