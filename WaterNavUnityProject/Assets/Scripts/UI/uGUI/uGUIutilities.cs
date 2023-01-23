using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UI.uGUI
{
    public static class uGUIutilities
    {
        private static HashSet<RectTransform> m_blockingElements = new HashSet<RectTransform>();

        public static void RegisterBlockingElement(RectTransform rectTransform) =>
            m_blockingElements.Add(rectTransform);

        public static void UnRegisterBlockingElement(RectTransform rectTransform) =>
            m_blockingElements.Remove(rectTransform);

        public static bool IsBlockingRayCasts(RectTransform rectTransform)
        {
            return m_blockingElements.Contains(rectTransform) && rectTransform.gameObject.activeSelf
                                                              && rectTransform.gameObject.layer != 7;
        }

        public static bool IsPointerOverUI(Vector2 ScreenPos)
        {
            foreach (var element in m_blockingElements)
            {
                if(!IsBlockingRayCasts(element))
                    continue;

                if (ContainsPoint(element, ScreenPos))
                    return true;
            }

            return false;
        }

        private static bool ContainsPoint(RectTransform element, Vector2 screenPos)
        {
            Vector2 scaledPosition = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            Vector2 flippedPosition = new Vector2(scaledPosition.x, 1 - scaledPosition.y);
            Vector2 adjustedPosition = flippedPosition * element.rect.size;

            Vector2 localPosition = element.TransformPoint(adjustedPosition);

            return element.rect.Contains(localPosition);
        }

#if UNITY_EDITOR
        [InitializeOnEnterPlayMode]
        private static void ResetBlockingElements()
        {
            m_blockingElements = new HashSet<RectTransform>();
        }
#endif
    }
}