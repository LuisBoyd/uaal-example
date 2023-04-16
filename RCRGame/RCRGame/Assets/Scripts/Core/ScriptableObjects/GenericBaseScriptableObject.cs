using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Utility;
using JsonSerializer = Utility.JsonSerializer;

namespace Core3.SciptableObjects
{
    public abstract class GenericBaseScriptableObject<T> : BaseScriptableObject where T : BaseScriptableObject
    {
        
        public GenericBaseScriptableObject() : base(){}

        public override void Initialize(BaseScriptableObject obj)
        {
            if (obj is T baseScriptableObject)
            {
                Initialize(baseScriptableObject);
            }
            else
            {
                Debug.LogWarning($"Failed to Initialize {typeof(T).GetNiceName()}");
            }
        }
        
        protected virtual void Initialize(T obj){}


#if UNITY_EDITOR
        [Button("To Json")]
        protected void SerializeToFile() => JsonSerializer.Serialize(Application.dataPath, this);

        [Button("From Json")]
        protected void DeserializeFromFile() => Initialize(JsonDeserializer.Deserialize<T>());
        
#endif
    }
}