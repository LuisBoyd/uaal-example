using RCRCoreLib.Core.Systems.Tutorial;

namespace RCRCoreLib.TutorialEvents
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