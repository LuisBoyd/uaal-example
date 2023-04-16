using System;
using System.Collections.Generic;
using Core3.SciptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Events
{
    [CreateAssetMenu(menuName = "RCR/Events/Void Event Channel")]
    public sealed class EventRelay : BaseScriptableObject
    {
        public UnityAction onEventRaised;

        public void RaiseEvent()
        {
            if(onEventRaised != null)
                onEventRaised.Invoke();
            else
                Debug.LogWarning("A Void Event was requested, but nobody picked it up.");
        }
    }
}