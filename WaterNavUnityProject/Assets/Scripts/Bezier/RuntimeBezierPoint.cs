using Events.Library.Models.BezierEvents;
using Input;
using NewManagers;
using PathCreation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bezier
{
    public class RuntimeBezierPoint : InteractableWorldObject
    {
        public override void OnPointerUp(PointerEventData eventData)
        {
            StartCoroutine(  GameManager_2_0.Instance.EventBus.Publish<On_BezierPointInteraction>(new On_BezierPointInteraction(),
                new On_BezierPointInteractionArgs(BezierPointState.Dropped)));
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            
           StartCoroutine( GameManager_2_0.Instance.EventBus.Publish<On_BezierPointInteraction>(new On_BezierPointInteraction(),
                new On_BezierPointInteractionArgs(BezierPointState.PickedUp)));
        }

        public override void OnDrag(PointerEventData eventData)
        {
            Vector3 updatedPosition = transform.localPosition + new Vector3(eventData.delta.x, eventData.delta.y);
            transform.localPosition = updatedPosition;
            //TODO Speed up movement by sensitivity
        }
    }
}