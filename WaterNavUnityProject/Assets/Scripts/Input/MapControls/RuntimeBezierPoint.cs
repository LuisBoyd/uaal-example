using UnityEngine;
using UnityEngine.EventSystems;

namespace RCR.Input.MapControls
{
    public class RuntimeBezierPoint : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerMoveHandler, IPointerClickHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log($"{name} was pointerUP");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"{name} was pointerDown");
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            Debug.Log($"{name} was pointerMove");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"{name} was Clicked");

        }
    }
}