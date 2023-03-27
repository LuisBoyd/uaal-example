using System.Collections;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.TutorialEvents;
using RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates;

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