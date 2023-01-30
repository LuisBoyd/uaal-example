using System;
using Events.Library.Models.InputEvent;
using NewManagers;
using RCR.Settings.NewScripts.Camera;
using RCR.Utilities;
using UI.uGUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

namespace Input.CameraInput
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Physics2DRaycaster))]
    public class CameraInput : PointerInputModule
    {
        private Camera m_camera;
        private Physics2DRaycaster m_physics2DRaycaster;
        private Vector2 _lastFramePosition = Vector2.zero;
        public GameCamera _gameCamera;
        public bool IgnoreProcess = false;

        [SerializeField]
        private float selectionRadius = 0.1f;

        public LayerMask layerMask;

        private class PointerData
        {
            public CameraInputEventData PointerEvent;
            public GameObject CurrentPoint;
            public GameObject CurrentPressed;
            public GameObject CurrentDragging;
        }
        public class CameraInputEventData : PointerEventData
        {
            public GameObject Current;

#if UNITY_EDITOR || UNITY_STANDALONE
            public Mouse mouse;
            public override void Reset()
            {
                Current = null;
                mouse = null;
                base.Reset();
            }
#elif UNITY_IOS || UNITY_ANDROID
            public Touch touch;
             public override void Reset()
            {
                Current = null;
                touch = null;
                base.Reset();
            }
#endif
            public CameraInputEventData(EventSystem eventSystem) : base(eventSystem){}

          
        }
        

        private PointerData m_data;

        protected override void Awake()
        {
            base.Awake();
            m_camera = GetComponent<Camera>();
            m_physics2DRaycaster = GetComponent<Physics2DRaycaster>();
            m_data = new PointerData();
            layerMask = LayerMask.NameToLayer("Water");
        }
        

        public override void Process()
        {
            if(IgnoreProcess)
                return;
            
            m_physics2DRaycaster.eventMask = layerMask; //TODO set layer mask
#if UNITY_ANDROID || UNITY_IOS
            Touch touch = input.touchCount > 0 ? input.GetTouch(0) : default;
#elif UNITY_EDITOR || UNITY_STANDALONE
            Mouse mouse = Mouse.current;
            Touch touch = input.touchCount > 0 ? input.GetTouch(0) : default;
#endif
            PointerData data = m_data;

            if (data.PointerEvent == null)
                data.PointerEvent = new CameraInputEventData(eventSystem);
            else
                data.PointerEvent.Reset();

#if UNITY_ANDROID || UNITY_IOS
            data.PointerEvent.touch = touch;
            data.PointerEvent.position = touch.position;
#elif UNITY_EDITOR || UNITY_STANDALONE
            data.PointerEvent.mouse = mouse;
            data.PointerEvent.position = mouse.position.ReadValue();
#endif
            data.PointerEvent.delta = mouse.delta.ReadValue() * Time.deltaTime;
            
            
            //Trigger a RayCast
            eventSystem.RaycastAll(data.PointerEvent, m_RaycastResultCache);
            data.PointerEvent.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
            m_RaycastResultCache.Clear();

            GameObject hitControl = data.PointerEvent.pointerCurrentRaycast.gameObject;

            if (data.CurrentPoint != hitControl)
            {
                if (data.CurrentPoint != null)
                {
                    //OnExit for a controller
                }

                if (hitControl != null)
                {
                    //OnEntry for a controller
                } 
            }

            data.CurrentPoint = hitControl;
            base.HandlePointerExitAndEnter(data.PointerEvent, data.CurrentPoint);
            bool didClickThisFrame = false;
            bool DidClickUpThisFrame = false;
            bool CurrentlyPressedThisFrame = false;

#if UNITY_ANDROID || UNITY_IOS
             if (input.touchCount > 0)
            {
                didClickThisFrame = touch.phase == TouchPhase.Began ? true : false;
                DidClickUpThisFrame = touch.phase == TouchPhase.Ended ? true : false;
                CurrentlyPressedThisFrame = touch.phase == TouchPhase.Moved ? true : false;
            }
#elif UNITY_EDITOR || UNITY_STANDALONE
            didClickThisFrame = mouse.leftButton.wasPressedThisFrame;
            DidClickUpThisFrame = mouse.leftButton.wasReleasedThisFrame;
            CurrentlyPressedThisFrame = mouse.leftButton.isPressed;
#endif
            
            if (didClickThisFrame)
            {
                ClearSelection();

                data.PointerEvent.pressPosition = data.PointerEvent.position;
                data.PointerEvent.pointerPressRaycast = data.PointerEvent.pointerCurrentRaycast;
                data.PointerEvent.pointerPress = null;

                //Update Current Pressed if the Pointer is over an element
                if (data.CurrentPoint != null)
                {
                    data.CurrentPressed = data.CurrentPoint;
                    bool pointerOverUI = uGUIutilities.IsPointerOverUI(data.PointerEvent.position);
                    if (!pointerOverUI)
                        data.CurrentPressed = SendWorldRaycast(data.PointerEvent.position);
                    data.PointerEvent.Current = data.CurrentPressed;
                    GameObject newPressed = ExecuteEvents.ExecuteHierarchy(data.CurrentPressed, data.PointerEvent,
                        ExecuteEvents.pointerDownHandler);
                    //ExecuteEvents.Execute(m_data.PointerEvent.Current) this is to execute an event on some controller if it implements the required interface
                    if (newPressed == null)
                    {
                        newPressed = ExecuteEvents.ExecuteHierarchy(data.CurrentPressed, data.PointerEvent,
                            ExecuteEvents.pointerClickHandler);
                        //ExecuteEvents.Execute() this is to execute an event on some controller if it implements the required interface
                        if (newPressed != null)
                        {
                            data.CurrentPressed = newPressed;
                        }
                    }
                    else
                    {
                        data.CurrentPressed = newPressed;

                        ExecuteEvents.Execute(newPressed, data.PointerEvent, ExecuteEvents.pointerClickHandler);
                        //ExecuteEvents.Execute() this is to execute an event on some controller if it implements the required interface
                    }

                    if (newPressed != null)
                    {
                        data.PointerEvent.pointerPress = newPressed;
                        data.CurrentPressed = newPressed;
                        Select(data.CurrentPressed);
                    }
                    
                    ExecuteEvents.Execute(data.CurrentPressed, data.PointerEvent, ExecuteEvents.beginDragHandler);
                    //ExecuteEvents.Execute(Controll.gameobject, data.PointerEvent, ExecuteEvents.beginDragHandler); this is to execute an event on some controller if it implements the required interface
                    data.PointerEvent.pointerDrag = data.CurrentPressed;
                    data.CurrentDragging = data.CurrentPressed;
                }
            }

            if (DidClickUpThisFrame)
            {
                if (data.CurrentDragging != null)
                {
                    data.PointerEvent.Current = data.CurrentDragging;
                    ExecuteEvents.Execute(data.CurrentDragging, data.PointerEvent, ExecuteEvents.endDragHandler);
                    //ExecuteEvents.Execute(Controller.gameobject, data.PointerEvent, ExecuteEvents.endDragHandler);
                    if (data.CurrentPoint != null)
                    {
                        ExecuteEvents.ExecuteHierarchy(data.CurrentPoint, data.PointerEvent, ExecuteEvents.dropHandler);
                    }
                    data.PointerEvent.pointerDrag = null;
                    data.CurrentDragging = null;
                }

                if (data.CurrentPressed)
                {
                    data.PointerEvent.Current = data.CurrentPressed;
                    ExecuteEvents.Execute(data.CurrentPressed, data.PointerEvent, ExecuteEvents.pointerUpHandler);
                    //ExecuteEvents.Execute(Controller.gameobject, data.PointerEvent, ExecuteEvents.pointerUpHandler);
                    data.PointerEvent.rawPointerPress = null;
                    data.PointerEvent.pointerPress = null;
                    data.CurrentPressed = null;
                }
            }

            if (data.CurrentDragging != null)
            {
                data.PointerEvent.Current = data.CurrentPressed;
                ExecuteEvents.Execute(data.CurrentDragging, data.PointerEvent, ExecuteEvents.dragHandler);
                //ExecuteEvents.Execute(Controller.gameobject, data.PointerEvent, ExecuteEvents.dragHandler);
            }

            if (data.CurrentDragging == null && CurrentlyPressedThisFrame)
            {
                ExecuteEvents.Execute(_gameCamera.gameObject, data.PointerEvent, ExecuteEvents.dragHandler);
            }

            if (base.eventSystem.currentSelectedGameObject != null)
            {
                data.PointerEvent.Current = eventSystem.currentSelectedGameObject;
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, GetBaseEventData(),
                    ExecuteEvents.updateSelectedHandler);
            }

            //_lastFramePosition = data.PointerEvent.position;

        }

        private GameObject SendWorldRaycast(Vector2 screenPos)
        {
            Vector2 location = m_camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y)); //Maybe Clipping Plane????? TODO clipping plane
            var hit2D = Physics2D.OverlapCircle(location, selectionRadius); //TODO layermask if I wanted
#if UNITY_EDITOR
            LBUtilities.DrawCircle(location, selectionRadius, 8, Color.red, 2f);
#endif
            if (hit2D != null)
            {
#if UNITY_EDITOR
               Debug.Log($"{hit2D.name} was hit");
#endif
                return hit2D.gameObject;
            }

            return null;
        }

        public new void ClearSelection()
        {
            if (base.eventSystem.currentSelectedGameObject)
            {
                base.eventSystem.SetSelectedGameObject(null);
            }
        }

        private void Select(GameObject go)
        {
            ClearSelection();
            if (ExecuteEvents.GetEventHandler<ISelectHandler>(go))
            {
                base.eventSystem.SetSelectedGameObject(go);
            }
        }
    }
}