using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RCR.Settings.SuperNewScripts.ScriptableObjects
{
    public abstract class AssetRefrenceLookup<T> : ScriptableObject where T : Object
    {
        [SerializeField]
        protected SerializableDictionary<T, AssetReferenceT<T>> m_lookUpDict =
            new SerializableDictionary<T, AssetReferenceT<T>>();
    }
}