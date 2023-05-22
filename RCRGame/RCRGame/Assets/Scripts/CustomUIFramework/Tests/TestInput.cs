using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomUIFramework.Tests
{
    public class TestInput : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"name {eventData.pointerClick.name}\n" +
                      $"this {this.name}");
        }
    }
}