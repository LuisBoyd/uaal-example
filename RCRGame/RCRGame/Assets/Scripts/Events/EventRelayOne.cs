using Core3.SciptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Events
{
    public abstract class EventRelayOne<T> : BaseScriptableObject
    {
        public UnityAction<T> onEventRaised;

        public void RaiseEvent(T arg0)
        {
            if(onEventRaised != null)
                onEventRaised.Invoke(arg0);
            else
                LogWarning();
        }
        public virtual void LogWarning()
        {
            Debug.LogWarning("A EventRelay<T0> event was raised, but " +
                             "nobody picked it up");
        }
    }
}