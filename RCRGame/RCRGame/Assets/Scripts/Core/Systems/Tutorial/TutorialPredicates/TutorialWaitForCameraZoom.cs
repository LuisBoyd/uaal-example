using UnityEngine;

namespace RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates
{
    public class TutorialWaitForCameraZoom : TutorialAction
    {
        private float Level;
        private float Limit;

        public override bool keepWaiting
        {
            get
            {
                CalculateZoom();
                if (Level >= Limit)
                    return false;
                return true;
            }
        }

        public TutorialWaitForCameraZoom(float limit)
        {
            Level = 0f;
            Limit = limit;
        }


        private void CalculateZoom()
        {
            if (UnityEngine.Input.touchCount == 2)
            {
                Touch touchZero = UnityEngine.Input.GetTouch(0);
                Touch touchOne = UnityEngine.Input.GetTouch(1);
                
                Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

                float distTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                float currentDistTouch = (touchZero.position - touchOne.position).magnitude;

                float difference = currentDistTouch - distTouch;

                //TODO sensitivity variable
                Level += Mathf.Abs(difference) * 0.01f;
                Debug.Log(Level);
            }
        }
    }
}