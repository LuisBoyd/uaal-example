using System.Collections;
using RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates;
using RCRCoreLib.TutorialEvents;

namespace RCRCoreLib.Core.Node.Nodes
{
    public abstract class TutorialNodeAction<T> : BaseTutorialNode where T : TutorialAction
    {
        public T action;

        public bool ShowSkipOrContinueButton;

        public override void Execute()
        {
            var showUIEvent = new ShowTutorialInterface(Message, LocationOnScreen, HorizontalFlipped, ShowSkipOrContinueButton);
            EventManager.Instance.QueueEvent(showUIEvent);
        }
        
        protected abstract IEnumerator WaitFor();
    }
}