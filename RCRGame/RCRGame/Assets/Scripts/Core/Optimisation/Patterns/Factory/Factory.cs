using Core3.MonoBehaviors;
using UnityEngine;

namespace Core.Optimisation.Patterns.Factory
{
    [DefaultExecutionOrder(-99)]
    public abstract class Factory<T> : BaseMonoBehavior, IFactory<T>
    {
        public abstract T Create();
    }
}