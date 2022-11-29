using System;
using UnityEngine;

namespace RCR.Physics
{
    public class CustomRaycaster: MonoBehaviour
    {
        private Camera m_eventCamera;
        private void Awake()
        {
            m_eventCamera = Camera.main;
        }

        public RaycastHit2D SendRayCast(Vector2 origin)
        {
            //TODO Implementation of Method
            return default;
        }
        

        /// <summary>
        /// Send the RayCast from the Center of the Event Camera
        /// </summary>
        /// <returns>RayCastHit</returns>
        public RaycastHit2D SendRayCast()
        {
            //TODO Implementation of Method
            return default;
        }
        
    }
}