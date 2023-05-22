using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CustomUIFramework.Organisms
{
    public class Slice : MonoBehaviour
    {
        [BoxGroup("Slice Properties")] 
        [Required] [SerializeField]
        private string sliceName;

        public string SliceName
        {
            get => sliceName;
        }

        [BoxGroup("Slice Properties")] 
        [Required] [SerializeField]
        private Camera BaseCamera;

        private UniversalAdditionalCameraData _additionalCameraData;

        [BoxGroup("Slice Properties/Camera Stacking")] 
        [SerializeField]
        private List<Camera> _camerasStack;

        private void Awake()
        {
            /*
             * Make sure Base camera is not null otherwise disable the game-object and component
             * get the additional camera data and assign it
             * notify user about no camera stack if there are less than 1 camera on the stack
             * if there is one or more iterate through the collection and assign them in order onto the stack.
             */
            if (BaseCamera == null)
            {
                Debug.LogWarning($"No Base Camera Assigned to {sliceName}/n" +
                                 $" disabling component");
                this.gameObject.SetActive(false);
            }
            _additionalCameraData = BaseCamera.GetUniversalAdditionalCameraData();
            if (_camerasStack.Count < 1)
            {
                Debug.Log($"{sliceName} has no additional camera data attached");
            }
            else
            {
                _camerasStack.ForEach(c => _additionalCameraData.cameraStack
                    .Add(c));
            }
        }
    }
}