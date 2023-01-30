using System;
using System.Collections;
using Cinemachine;
using Events.Library.Models;
using Events.Library.Models.InputEvent;
using Events.Library.Models.WorldEvents;
using Input.CameraInput;
using NewManagers;
using RCR.Settings.NewScripts.View;
using RCR.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace RCR.Settings.NewScripts.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera), typeof(CinemachineConfiner2D))]
    public class GameCamera : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Varibles

        private CinemachineVirtualCamera VirtualCamera;
        private CinemachineConfiner2D Confiner2D;
        private CameraInput CameraInput;

        private Transform helper;
        private Token BoundsUpdatedtoken;
        public float ZoomInMax = 2;
        public float ZoomOutMax = 6;
        public float ZoomSensitivty = 1;
        
        public float Sensitivity = 1;
        
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        [SerializeField] 
        private float ScrollDeadZoneRange = 0.02f;
        #endif

        #endregion
        
        private void Awake()
        {
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            Confiner2D = GetComponent<CinemachineConfiner2D>();
            CameraInput = UnityEngine.Camera.main.GetComponent<CameraInput>();
            CameraInput._gameCamera = this;
            helper = new GameObject($"{VirtualCamera.name}_helper").transform;
            //helper.SetParent(VirtualCamera.transform);
            
             VirtualCamera.Follow = helper;
             VirtualCamera.m_Lens.OrthographicSize = ZoomOutMax; 
             BoundsUpdatedtoken = GameManager_2_0.Instance.EventBus.Subscribe<WorldBoundsChanged>(Update_CameraBounds);
        }

        private void OnDisable()
        {
            GameManager_2_0.Instance.EventBus.UnSubscribe<WorldBoundsChanged>(BoundsUpdatedtoken.TokenId);
        }


#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private void Update()
        {
            float zoom = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            if (zoom < -ScrollDeadZoneRange || zoom > ScrollDeadZoneRange) //Deadzones
            {
                Debug.Log($"Zoom Levels: {zoom}");
                ZoomScreen(zoom);
            }
        }
#endif

        private void Update_CameraBounds(WorldBoundsChanged evnt, EventArgs args)
        {
            WorldBoundsChangedArgs worldBoundsChangedArgs;
            if (!LBUtilities.Cast<WorldBoundsChangedArgs>(args, out worldBoundsChangedArgs))
                return;

            Confiner2D.m_BoundingShape2D = WorldView.WorldBoundsCollider;
            Confiner2D.InvalidateCache();
            if (!Confiner2D.m_BoundingShape2D.OverlapPoint(helper.position))
            {
                Vector3 prevPosition = helper.position;
                helper.position = worldBoundsChangedArgs.ChunkBounds.center;
                var delta = helper.position - prevPosition;
                VirtualCamera.OnTargetObjectWarped(helper, delta);
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private void UpdateCamera_Pointer(PointerEventData eventData)
        {
            Vector3 NewPosition = helper.localPosition + new Vector3(eventData.delta.x, eventData.delta.y) * Sensitivity;
            if(!Confiner2D.m_BoundingShape2D.OverlapPoint(NewPosition))
                return;
            helper.localPosition += new Vector3(eventData.delta.x, eventData.delta.y) * Sensitivity;
            Debug.Log(helper.localPosition);
        }
        
#elif UNITY_IOS || UNITY_ANDROID
        
       private void UpdateCamera_Touch()
        {
            if (UnityEngine.Input.touchCount != 0)
            {
                Vector3 localDelta = UnityEngine.Input.touches[0].deltaPosition;
                helper.localPosition += localDelta;

                if (UnityEngine.Input.touchCount == 2)
                {
                    Vector3 SecondFingerLocalDelta = UnityEngine.Input.touches[1].deltaPosition;

                    Vector2 Touch0prev = UnityEngine.Input.touches[0].position - new Vector2(localDelta.x,localDelta.y);
                    Vector2 Touch1prev = UnityEngine.Input.touches[1].position -
                                         new Vector2(SecondFingerLocalDelta.x, SecondFingerLocalDelta.y);

                    float prevTocuhDeltaMag = (Touch0prev - Touch1prev).magnitude;
                    float touchDeltaMag =
                        (UnityEngine.Input.touches[0].position - UnityEngine.Input.touches[1].position).magnitude;

                    float deltaMagnitudeDiff = prevTocuhDeltaMag - touchDeltaMag;
                    ZoomScreen(deltaMagnitudeDiff);
                }
                
            }
        }
#endif

        private void ZoomScreen(float zoomlevel)
        {
            float fov = VirtualCamera.m_Lens.OrthographicSize;
            float target = Mathf.Clamp((fov + (zoomlevel)), ZoomInMax, ZoomOutMax);
            VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(fov, target, ZoomSensitivty * Time.deltaTime);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
          
        }

        public void OnDrag(PointerEventData eventData)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            UpdateCamera_Pointer(eventData);
#elif UNITY_IOS || UNITY_ANDROID
            UpdateCamera_Touch();
#endif
        }

        public void OnEndDrag(PointerEventData eventData)
        {
           
        }
    }
}