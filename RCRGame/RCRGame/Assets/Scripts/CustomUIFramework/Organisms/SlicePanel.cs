using System;
using CustomUIFramework.Animation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomUIFramework.Organisms
{
    [RequireComponent(typeof(RectTransform))]
    public class SlicePanel : MonoBehaviour
    {
        [SerializeField] [ChildGameObjectsOnly]
        private AnimationTargetEventHandler _animationTargetEventHandler;

        public RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }
}