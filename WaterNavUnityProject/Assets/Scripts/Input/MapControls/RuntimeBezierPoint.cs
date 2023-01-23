using Input;
using PathCreation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input.MapControls
{
    public class RuntimeBezierPoint : InteractableWorldObject
    {
        
        public BezierPath Path { get; set; }
        public int Index { get; set; }

        public override void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log($"{name} was pointerUP");
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"{name} was pointerDown");
        }

        public override void OnDrag(PointerEventData eventData)
        {
            Vector3 updatedPosition = transform.localPosition + new Vector3(eventData.delta.x, eventData.delta.y);
            Debug.Log(eventData.delta);
            //Debug.Log(updatedPosition);
            Path.MovePoint(Index, updatedPosition);
        }
    }
}