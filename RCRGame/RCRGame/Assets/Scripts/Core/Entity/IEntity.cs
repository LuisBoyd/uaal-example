using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Entity
{
    public interface IEntity
    {
        public Transform Transform { get; }
        UniTaskVoid Moveto(Vector3 position, Action onArrival = null);
    }
}