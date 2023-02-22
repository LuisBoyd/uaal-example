using UnityEngine;

namespace RCRCoreLib.Core.Optimisation.Patterns.Factory
{
    [DefaultExecutionOrder(-99)]
    public abstract class Factory<T> : MonoBehaviour, IFactory<T>
    {
        public abstract T Create();
    }
}