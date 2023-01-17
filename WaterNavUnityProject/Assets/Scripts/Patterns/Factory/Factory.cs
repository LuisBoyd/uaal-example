using UnityEngine;

namespace Patterns.Factory
{

    [DefaultExecutionOrder(-99)]
    public abstract class Factory<T> : MonoBehaviour, IFactory<T>
    {
        public abstract T Create();
       


        public abstract T Clone(T original);

    }
}