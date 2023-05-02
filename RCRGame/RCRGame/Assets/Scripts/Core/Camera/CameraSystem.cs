using System;
using Cinemachine;
using Core3.MonoBehaviors;
using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;
using VContainer;

namespace Core.Camera
{
    public class CameraSystem : BaseMonoBehavior
    {
        [Title("Camera Settings")] 
        [Required] [SerializeField]
        private CinemachineVirtualCamera _VCamera;
        [SerializeField] [Required]
        private Transform _VCameraFollowTarget;
        [SerializeField] [Required] 
        private PolygonCollider2D _WorldBoundries;
        [SerializeField] [Required] private BoolEventChannelSO _CameraSwitchActiveStateEvent;
        [PropertyRange(1f, 10f)]
        [SerializeField] private float MinOrthognathicSize = 2f;
        [PropertyRange(10f, 20f)]
        [SerializeField] private float MaxOrthognathicSize = 10f;
        [PropertyRange(0f,1f)]
        [SerializeField] private float ZoomSensitivity = 0.01f;
        [PropertyRange(0f,1f)]
        [SerializeField] private float FocusSpeed = 0.2f;

        
        private UnityEngine.Camera _MainCamera;
        private bool CanVCamMove
        {
            get => _VCamera.Follow != null;
        }
        private bool CanMove { get; set; }
        private bool AllowMovement { get; set; } = true;

        private Vector3 _currentTouchPosition;

        private LTDescr _currentLeanTransition;

        private void Awake()
        {
            if (_VCamera == null)
                _VCamera = FindObjectOfType<CinemachineVirtualCamera>(); //This should find the virtual camera in the scene a bit expensive though.
            if(_MainCamera == null)
                _MainCamera = UnityEngine.Camera.main;
            if (_VCameraFollowTarget == null)
            {
                _VCameraFollowTarget = new GameObject("Game-VirtualCameraTarget").transform;
                _VCamera.Follow = _VCameraFollowTarget;
            }
            if (_VCamera.Follow == null)
                _VCamera.Follow = _VCameraFollowTarget;

            _MainCamera.orthographic = true; //make sure camera is in orthographic mode.
            _VCamera.m_Lens.OrthographicSize = (MinOrthognathicSize + MaxOrthognathicSize) / 2f; //Get Mid Value between max and min.
        }

        private void OnEnable()
        {
            _CameraSwitchActiveStateEvent.onEventRaised += On_Camera_Switch_Active_State;
        }

        private void OnDisable()
        {
            _CameraSwitchActiveStateEvent.onEventRaised -= On_Camera_Switch_Active_State;
        }

        private void On_Camera_Switch_Active_State(bool state)
        {
            AllowMovement = state;
        }

        [Inject]
        private void Inject(UnityEngine.Camera mainCamera)
        {
            _MainCamera = mainCamera;
        }

        private void Update()
        {
            if(!CanVCamMove || !AllowMovement)
                return;
            if (Input.touchCount > 0)
            {
                Touch mainTouch = Input.GetTouch(0);
                if (Input.touchCount == 2)
                {
                    Touch indexTouch = Input.GetTouch(1);
                    if(EventSystem.current.IsPointerOverGameObject(mainTouch.fingerId)
                       || EventSystem.current.IsPointerOverGameObject(indexTouch.fingerId))
                        return;

                    Vector2 mainTouchLastPos = mainTouch.position - mainTouch.deltaPosition;
                    Vector2 indexTouchLastPos = indexTouch.position - indexTouch.deltaPosition;

                    float deltaDistanceBetweenFingers = (mainTouchLastPos - indexTouchLastPos).magnitude;
                    float distanceBetweenFingers = (mainTouch.position - indexTouch.position).magnitude;

                    float delta = distanceBetweenFingers - deltaDistanceBetweenFingers;
                    
                    Zoom(delta * ZoomSensitivity);
                }
                else
                {
                    switch (mainTouch.phase)
                    {
                        case TouchPhase.Began:
                            CanMove = BeginMovement(mainTouch.fingerId);
                            _currentTouchPosition = _MainCamera.ScreenToWorldPoint(mainTouch.position);
                            break;
                        case TouchPhase.Moved:
                            ContinueMovement(mainTouch);
                            break;
                    }
                }
            }
        }

        private bool BeginMovement(int fingerID)
        {
            if (EventSystem.current.IsPointerOverGameObject(fingerID))
                return false;
            return true;
        }

        private void ContinueMovement(Touch touch)
        {
            if(!CanMove)
                return;

            Vector3 direction = _currentTouchPosition - _MainCamera.ScreenToWorldPoint(touch.position);
            if (_WorldBoundries.PointInside(_VCameraFollowTarget.transform.position + direction))
            {
                _VCameraFollowTarget.transform.position += direction;
            }
        }



        private void Zoom(float zoomValue)
        {
            _VCamera.m_Lens.OrthographicSize = Mathf.Clamp(_VCamera.m_Lens.OrthographicSize - zoomValue,
                MinOrthognathicSize, MaxOrthognathicSize);
        }

        public void FollowTarget(Transform target) => _VCamera.Follow = target;
        public void UnfollowTarget()
        {
            _VCameraFollowTarget.transform.position = _VCamera.transform.position;
            _VCamera.Follow = _VCameraFollowTarget;
        }
        public void Focus(Vector3 position)
        {
            Focus(new Vector2(position.x,position.y));
        }
        public void Focus(Vector2 position)
        {
            if (_WorldBoundries.PointInside(position))
            {
                _currentLeanTransition = LeanTween.move(_VCameraFollowTarget.gameObject, position, FocusSpeed);
            }
        }


    }
}