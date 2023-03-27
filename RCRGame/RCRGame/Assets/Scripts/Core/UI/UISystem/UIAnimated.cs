using System;
using UnityEngine;

namespace RCRCoreLib.Core.UI.UISystem
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIAnimated : MonoBehaviour
    {
        [SerializeField] 
        protected float time_to_lerp;

        protected RectTransform selfTransform;

        protected virtual void Awake()
        {
            selfTransform = GetComponent<RectTransform>();
        }

        public abstract void Open();
        public abstract void Close();
    }
}