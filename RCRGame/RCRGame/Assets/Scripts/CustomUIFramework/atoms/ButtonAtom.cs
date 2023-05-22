using System;
using CustomUIFramework.enums;
using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CustomUIFramework.atoms
{
    public class ButtonAtom : Atom, IPointerDownHandler, IPointerUpHandler
    {
        [EnumToggleButtons] [SerializeField] 
        private EventMessagingType _eventMessagingType; //how do we send messages in this atom??

        [ShowIfGroup("_eventMessagingType", EventMessagingType.UnityAction)]
        [BoxGroup("Unity Actions")]
        public UnityAction OnClickAction = default;

        [ShowIfGroup("_eventMessagingType", EventMessagingType.SObjects)]
        [BoxGroup("SO Actions")]
        public EventRelay OnClickSo = default;

        public  void OnPointerDown(PointerEventData eventData)
        {
           _animator.SetBool(Animator.StringToHash("Pressed"), true);
           _animator.SetTrigger(Animator.StringToHash("Clicked"));

           switch (_eventMessagingType)
            {
                case EventMessagingType.SObjects:
                    if(OnClickSo != null)
                        OnClickSo.RaiseEvent();
                    break;
                case EventMessagingType.UnityAction:
                    if(OnClickAction != null)
                        OnClickAction();
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _animator.SetBool(Animator.StringToHash("Pressed"), false);
        }
    }
}