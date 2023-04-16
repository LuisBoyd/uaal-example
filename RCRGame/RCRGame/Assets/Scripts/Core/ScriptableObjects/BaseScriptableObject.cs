using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core3.SciptableObjects
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
        public Guid id { get; private set; } //Should Not Change..
        [SerializeField] [ReadOnly] protected string id_display;
        public BaseScriptableObject()
        {
            if (id == null || id == Guid.Empty)
            {
                id = Guid.NewGuid();
                id_display = id.ToString();
            }
        }
        
        public virtual void Initialize(BaseScriptableObject obj){}
        
        
    }
}