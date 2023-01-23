using System;
using UnityEngine;

namespace UI.uGUI
{
    [RequireComponent(typeof(RectTransform))]
    public class uGUIBlockingElement : MonoBehaviour
    {
        private RectTransform _personalRectTransform;

        private void Awake() => _personalRectTransform = GetComponent<RectTransform>();

        private void OnEnable() => uGUIutilities.RegisterBlockingElement(_personalRectTransform);

        private void OnDisable() => uGUIutilities.UnRegisterBlockingElement(_personalRectTransform);
    }
}