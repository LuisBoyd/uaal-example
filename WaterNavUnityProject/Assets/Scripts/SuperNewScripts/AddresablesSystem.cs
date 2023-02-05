using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace RCR.Settings.SuperNewScripts
{
    public class AddresablesSystem : MultithreadedSafeSingelton<AddresablesSystem>
    {
        private UniTask<OperationResult<IResourceLocator>> _AddressableSystemInitHandle;
        private Dictionary<AssetLabelReference, IResourceLocation> _AssetLabelsResourceLocations;

        public AddresablesSystem()
        {
        }

        public async UniTaskVoid Start()
        {
            //Initialise the Addressable System and assign the Handle to a local varible and release the main one.
            _AddressableSystemInitHandle = AddressablesManager.InitializeAsync(true);
            await _AddressableSystemInitHandle;
            Debug.Log($"{_AddressableSystemInitHandle.Status}");
        }

        public async UniTaskVoid LoadAssetLabel(IEnumerable<AssetLabelReference> labels)
        {
            Debug.Log(AddressablesManager.Keys);
            var Locations = AddressablesManager.LoadLocationsAsync(labels.First());
            await Locations;
            Debug.Log(Locations);
        }
    }
}