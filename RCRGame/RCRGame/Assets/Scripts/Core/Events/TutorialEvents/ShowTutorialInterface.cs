using UnityEngine;

namespace RCRCoreLib.TutorialEvents
{
    public class ShowTutorialInterface : TutorialEvent
    {
        public string Message;
        public bool HorizontalFlipped;
        public Vector2 LocationOnScreen;

        public ShowTutorialInterface(string message, Vector2 ScreenPos, bool HorizontalFlipped = false)
        {
            this.HorizontalFlipped = HorizontalFlipped;
            this.Message = message;
            this.LocationOnScreen = LocationOnScreen;
        }
    }
}