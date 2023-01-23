using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public class InteractableWorldObject :  MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    ,IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
    {
        public virtual void OnPointerUp(PointerEventData eventData)
        {
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
        }
    }
}