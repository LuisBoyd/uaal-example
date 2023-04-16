using Core3.SciptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Events
{
    public abstract class EventRelayFour<T0,T1,T2,T3> : BaseScriptableObject
    {
        public UnityAction<T0,T1,T2, T3> onEventRaised;

        public void RaiseEvent(T0 arg0, T1 arg1, T2 arg2,T3 arg3)
        {
            if(onEventRaised != null)
                onEventRaised.Invoke(arg0,arg1,arg2,arg3);
            else
                LogWarning();
        }
        public virtual void LogWarning()
        {
            Debug.LogWarning("A EventRelay<T0,T1,T2,T3> event was raised, but " +
                             "nobody picked it up");
        }
    }
}