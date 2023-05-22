using System;
using CustomUIFramework.enums;
using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CustomUIFramework.atoms
{
    public class ButtonAtom : Atom
    {
        [EnumToggleButtons] [SerializeField] 
        private EventMessagingType _eventMessagingType; //how do we send messages in this atom??

        [ShowIfGroup("_eventMessagingType", EventMessagingType.UnityAction)]
        [BoxGroup("Unity Actions")]
        public UnityAction OnClickAction = default;

        [ShowIfGroup("_eventMessagingType", EventMessagingType.SObjects)]
        [BoxGroup("SO Actions")]
        public EventRelay OnClickSo = default;

        public override void OnPointerDown(PointerEventData eventData)
        {
            _animationTargetEventHandler.AnimationTriggerSO.RaiseEvent("ClickedState");
            
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

        public override void OnPointerUp(PointerEventData eventData)
        {
            _animationTargetEventHandler.AnimationTriggerSO.RaiseEvent("UnClickedState");
        }
    }
}