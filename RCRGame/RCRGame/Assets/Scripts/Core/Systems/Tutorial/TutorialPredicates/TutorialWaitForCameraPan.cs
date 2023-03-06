using UnityEngine;

namespace RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates
{
    public class TutorialWaitForCameraPan : TutorialAction
    {
        private Vector2 Position;
        private float Limit;

        public override bool keepWaiting
        {
            get
            {
                CalculateMovement();
                if (Position.x >= Limit && Position.y >= Limit)
                {
                    return false;
                }
                return true;
            }
        }

        public TutorialWaitForCameraPan(float limit)
        {
            Position = Vector2.zero;
            Limit = limit;
        }

        private void CalculateMovement()
        {
            if (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.touchCount < 2)
            {
                Debug.Log("Touch 1");
                Touch touch = UnityEngine.Input.GetTouch(0);
                Position += touch.deltaPosition;
            }
        }
    }
}