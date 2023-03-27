using RCRCoreLib.Core.Systems.Tutorial;

namespace RCRCoreLib.Core.Events.TutorialEvents
{
    public class StartTutorialEvent : TutorialEvent
    {
        public TutorialType type;

        public StartTutorialEvent(TutorialType tutorialType)
        {
            type = tutorialType;
        }
    }
}