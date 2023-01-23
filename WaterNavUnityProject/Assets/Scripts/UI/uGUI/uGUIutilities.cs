using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UI.uGUI
{
    public static class uGUIutilities
    {
        private static HashSet<RectTransform> m_blockingElements = new HashSet<RectTransform>();
        private static Dictionary<RectTransform, Canvas> m_owningCanvas = new Dictionary<RectTransform, Canvas>();

        public static void RegisterBlockingElement(RectTransform rectTransform)
        {
            m_blockingElements.Add(rectTransform);
            m_owningCanvas.Add(rectTransform, GetCanvas(rectTransform));
        }

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

            //naturalRect is fine but when I grab the rect they may be in local/screen position where screenpos is in world/Screen

            Vector3 MinWorldScreen = element.TransformPoint(element.rect.min);
            Vector3 MaxWorldScreen = element.TransformPoint(element.rect.max);
            
            Rect screenRect = Rect.MinMaxRect(MinWorldScreen.x, MinWorldScreen.y,
                MaxWorldScreen.x, MaxWorldScreen.y);

            return screenRect.Contains(screenPos);

            // float rectWidth = GetWidthScreen(element);
            // float rectHeight = GetHeightScreen(element);
            //
            // Vector2 position = new Vector2(element.anchorMin.x * Screen.width,
            //     element.anchorMin.y * Screen.height);
            //
            // Rect screenRect = new Rect(position, new Vector2(rectWidth, rectHeight));
            //
            // return screenRect.Contains(screenPos);

            // Vector2 scaledPosition = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            // Vector2 flippedPosition = new Vector2(scaledPosition.x, 1 - scaledPosition.y);
            // Vector2 adjustedPosition = flippedPosition * element.rect.size;
            //
            // Vector2 localPosition = element.TransformPoint(adjustedPosition);
            //
            // return element.rect.Contains(localPosition);
        }

        private static Canvas GetCanvas(RectTransform t)
        {
            return t.gameObject.GetComponentInParent<Canvas>();
        }
        
        private static float GetHeightScreen(RectTransform t)
        {
            var h = (t.anchorMax.y - t.anchorMin.y) * Screen.height + t.sizeDelta.y * m_owningCanvas[t].scaleFactor;
            return h;
        }

        private static float GetWidthScreen(RectTransform t)
        {
            var w = (t.anchorMax.x - t.anchorMin.x) * Screen.width + t.sizeDelta.x * m_owningCanvas[t].scaleFactor;
            return w;
        }

#if UNITY_EDITOR
        [InitializeOnEnterPlayMode]
        private static void ResetBlockingElements()
        {
            m_blockingElements = new HashSet<RectTransform>();
            m_owningCanvas = new Dictionary<RectTransform, Canvas>();
        }
#endif
    }
}