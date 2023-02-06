using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace RCR.Settings.SuperNewScripts
{
    public class AddresablesSystem : MultithreadedSafeSingelton<AddresablesSystem>, IProgress<float>
    {
        private OperationResult<IResourceLocator> _AddressableSystemInitHandle;
        private Dictionary<AssetLabelReference, IResourceLocation> _AssetLabelsResourceLocations;

        public AddresablesSystem()
        {
            
        }

        public async UniTask<OperationResult<IResourceLocator>> Start()
        {
            _AddressableSystemInitHandle = await AddressablesManager.InitializeAsync(this, false);
            return _AddressableSystemInitHandle;
        }

        public async UniTask<OperationResult<SceneInstance>> LoadSceneAsync(string key,
            LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            int priority = 100)
        {
           return await AddressablesManager.LoadSceneAsync(key, loadMode, activateOnLoad, priority);
        }
        
        public async UniTask<OperationResult<SceneInstance>> LoadSceneAsync(AssetReference reference,
            LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            int priority = 100)
        {
            return await AddressablesManager.LoadSceneAsync(reference, loadMode, activateOnLoad, priority);
        }

        public async UniTask<OperationResult<T>> LoadAssetAsync<T>(string key) where T : Object
        {
            return await AddressablesManager.LoadAssetAsync<T>(key);
        }
        public async UniTask<OperationResult<T>> LoadAssetAsync<T>(AssetReferenceT<T> reference) where T : Object
        {
            return await AddressablesManager.LoadAssetAsync<T>(reference);
        }

        public static async UniTask<OperationResult<GameObject>> InstantiateAsync(string key,
            Transform parent = null,
            bool inWorldSpace = false,
            bool trackHandle = true)
        {
           return await AddressablesManager.InstantiateAsync(key, parent, inWorldSpace, trackHandle);
        }

        public static async UniTask<OperationResult<GameObject>> InstantiateAsync(AssetReference reference,
            Transform parent = null,
            bool inWorldSpace = false)
        {
            return await AddressablesManager.InstantiateAsync(reference, parent, inWorldSpace);
        }

        public async UniTaskVoid LoadAssetLabel(IEnumerable<AssetLabelReference> labels)
        {
            // Debug.Log(AddressablesManager.Keys);
            // var Locations = AddressablesManager.LoadLocationsAsync(labels.First());
            // await Locations;
            // Debug.Log(Locations);
        }

        public string GetInstanceAddress(object obj)
        {
            return AddressablesManager.GetInstanceAddress(obj);
        }

        public void Report(float value)
        {
            Debug.Log(value);
        }

        private void Complete()
        {
            // Debug.Log(AddressablesManager.Keys.Count);
        }
        private void Failed()
        {
            Debug.Log("Failed Init");
        }
        
    }
}