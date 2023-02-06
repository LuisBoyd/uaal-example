using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine.AddressableAssets;

namespace RCR.Settings.SuperNewScripts.SaveSystem.RefrenceResolvers
{
    public class AddressableAssetRefrenceResolver : IReferenceResolver
    {
        private readonly IDictionary<string, AssetReference> _objects = new Dictionary<string, AssetReference>();

        public object ResolveReference(object context, string reference)
        {
            AssetReference assetReference;
            if (_objects.TryGetValue(reference, out assetReference))
            {
                return assetReference;
            }

            return null;
        }

        public string GetReference(object context, object value)
        {
            AssetReference assetReference = (AssetReference)value;
            _objects[assetReference.RuntimeKey.ToString()] = assetReference;

            return assetReference.RuntimeKey.ToString();
        }

        public bool IsReferenced(object context, object value)
        {
            AssetReference assetReference = (AssetReference)value;

            return _objects.ContainsKey(assetReference.RuntimeKey.ToString());
        }

        public void AddReference(object context, string reference, object value)
        {
            _objects.Add(reference, (AssetReference)value);
        }

        public static AddressableAssetRefrenceResolver Create()
        {
            return new AddressableAssetRefrenceResolver();
        }

        public static AddressableAssetRefrenceResolver Create(IEnumerable<KeyValuePair<string, AssetReference>> KVP)
        {
            AddressableAssetRefrenceResolver resolver = new AddressableAssetRefrenceResolver();
            foreach (var keyValuePair in KVP)
            {
                resolver.AddReference(default,keyValuePair.Key, keyValuePair.Value);
            }

            return resolver;
        }
        public static AddressableAssetRefrenceResolver Create(IDictionary<string, AssetReference> KVP)
        {
            AddressableAssetRefrenceResolver resolver = new AddressableAssetRefrenceResolver();
            foreach (var keyValuePair in KVP)
            {
                resolver.AddReference(default,keyValuePair.Key, keyValuePair.Value);
            }

            return resolver;
        }
    }
}