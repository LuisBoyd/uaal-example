using Core3.SciptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Events
{
    public abstract class EventRelayTwo<T0,T1> : BaseScriptableObject
    {
        public UnityAction<T0,T1> onEventRaised;

        public void RaiseEvent(T0 arg0, T1 arg1)
        {
            if(onEventRaised != null)
                onEventRaised.Invoke(arg0,arg1);
            else
                LogWarning();
        }
        public virtual void LogWarning()
        {
            Debug.LogWarning("A EventRelay<T0,T1> event was raised, but " +
                             "nobody picked it up");
        }
    }
}