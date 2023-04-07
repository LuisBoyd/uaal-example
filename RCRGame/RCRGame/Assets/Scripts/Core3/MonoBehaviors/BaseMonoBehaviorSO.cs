using System;
using Core3.SciptableObjects;
using UnityEngine;

namespace Core3.MonoBehaviors
{
    public abstract class BaseMonoBehaviorSO<T> : BaseMonoBehavior, IDisposable where T : BaseScriptableObject
    {
        public T scriptableObject;
        
        public virtual void Init(T scriptableObject)
        {
            this.scriptableObject = scriptableObject;
        }
        
        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}