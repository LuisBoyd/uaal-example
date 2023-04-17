using System;
using Core3.SciptableObjects;
using UnityEngine;

namespace Core3.MonoBehaviors
{
    public abstract class BaseMonoBehavior : MonoBehaviour, IDisposable 
    {
        public virtual void Dispose()
        {
        }
    }
}