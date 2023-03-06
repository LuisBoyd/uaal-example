using System;
using Cinemachine;
using RCRCoreLib.Core.Timers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.CameraLib
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PanZoom: Singelton<PanZoom>
    {
        private CinemachineVirtualCamera camera;
        private Camera mainCamera;

        private bool moveAllowed;
        public bool MoveEnabled;

        [SerializeField] private float LeftLimit;
        [SerializeField] private float RightLimit;
        [SerializeField] private float BottomLimit;
        [SerializeField] private float UpperLimit;
        
        [SerializeField] private float zoomMin;
        [SerializeField] private float zoomMax;

        private Transform objectToFollow;
        private Bounds objectBounds;
        private Vector3 prevPos;

        private Vector3 touchPos;

        protected override void Awake()
        {
            base.Awake();
            camera = GetComponent<CinemachineVirtualCamera>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (objectToFollow != null)
            {
                // Vector3 objPos = camera.WorldToViewportPoint(objectToFollow.position + objectBounds.max);
                // if (objPos.x >= 0.7f || objPos.x <= 0.3f || objPos.y >= 0.7f || objPos.y <= 0.3f)
                // {
                //     Vector3 pos = camera.ScreenToWorldPoint(objectToFollow.position);
                //     Vector3 direction = pos - prevPos;
                //     camera.transform.position += direction;
                //     prevPos = pos;
                //     
                //     transform.position = new Vector3(
                //         Mathf.Clamp(transform.position.x, LeftLimit, RightLimit),
                //         Mathf.Clamp(transform.position.y, BottomLimit, UpperLimit),
                //         transform.position.z
                //     );
                // }
                // else
                // {
                //     Vector3 pos = camera.ScreenToWorldPoint(objectToFollow.position);
                //     prevPos = pos;
                // }
                Debug.Log("Object To Follow");
                return;
            }
            
            if(!MoveEnabled)
                return;

            if (UnityEngine.Input.touchCount > 0)
            {
               
                if (UnityEngine.Input.touchCount == 2)
                {
                   
                    Touch touchZero = UnityEngine.Input.GetTouch(0);
                    Touch touchOne = UnityEngine.Input.GetTouch(1);

                    if (EventSystem.current.IsPointerOverGameObject(touchOne.fingerId)
                        || EventSystem.current.IsPointerOverGameObject(touchZero.fingerId))
                    {
                        return;
                    }

                    Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

                    float distTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                    float currentDistTouch = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentDistTouch - distTouch;
                    
                    Zoom(difference * 0.01f); //TODO sensitivity variable
                }
                else
                {
                    Touch touch = UnityEngine.Input.GetTouch(0);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                            {
                                Debug.Log("Touch Over Object Not allowed");
                                moveAllowed = false;
                            }
                            else
                            {
                                moveAllowed = true;
                                //Hide ToolTip TODO other DYNAMIC UI
//                                TimerToolTip.HideTimer_Static();
                            }

                            touchPos = mainCamera.ScreenToWorldPoint(touch.position);
                            break;
                        case TouchPhase.Moved:
                            if (moveAllowed)
                            {
                                Vector3 direction = touchPos - mainCamera.ScreenToWorldPoint(touch.position);
                                camera.transform.position += direction;
                                
                                transform.position = new Vector3(
                                    Mathf.Clamp(transform.position.x, LeftLimit, RightLimit),
                                    Mathf.Clamp(transform.position.y, BottomLimit, UpperLimit),
                                    transform.position.z
                                    );
                            }
                            break;
                    }
                }
            }
        }

        private void Zoom(float increment)
        {
            camera.m_Lens.OrthographicSize = Mathf.Clamp( camera.m_Lens.OrthographicSize - increment, zoomMin, zoomMax);
        }

        public void FollowObject(Transform objToFollow)
        {
            camera.Follow = objToFollow;
            camera.LookAt = objToFollow;
            //prevPos = camera.ScreenToWorldPoint(Vector3.zero);
            //Focus(objToFollow.position);
        }

        public void UnfollowObject()
        {
            camera.Follow = null;
            camera.LookAt = null;
        }

        public void Focus(Vector3 position)
        {
            Vector3 newPos = new Vector3(position.x, position.y, transform.position.z);
            LeanTween.move(gameObject, newPos, 0.2f);
            
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, LeftLimit, RightLimit),
                Mathf.Clamp(transform.position.y, BottomLimit, UpperLimit),
                transform.position.z
            );
            touchPos = transform.position;
        }

        private void OnDrawGizmos()
        {
           Gizmos.color = Color.yellow;
           Gizmos.DrawWireCube(
               new Vector3(
                   (RightLimit - Mathf.Abs(LeftLimit)/2.0f),
                   (UpperLimit - Mathf.Abs(BottomLimit)/2.0f)),
               new Vector3(RightLimit - LeftLimit, UpperLimit - BottomLimit)
               );
        }
    }
}