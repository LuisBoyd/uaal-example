using UnityEngine;

namespace RCRCoreLib.Core.Events.TutorialEvents
{
    public class ShowTutorialInterface : TutorialEvent
    {
        public string Message;
        public bool SkipOrContinue;
        public bool HorizontalFlipped;
        public Vector2 LocationOnScreen;

        public ShowTutorialInterface(string message, Vector2 ScreenPos, bool HorizontalFlipped = false, bool ShowSkipOrContine = true)
        {
            this.HorizontalFlipped = HorizontalFlipped;
            this.SkipOrContinue = ShowSkipOrContine;
            this.Message = message;
            this.LocationOnScreen = LocationOnScreen;
        }
    }
}