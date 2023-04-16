using System;
using Core3.MonoBehaviors;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public class InitializationLoader : BaseMonoBehavior
    {
        [SerializeField] private SceneSO _managersScene = default;
        [SerializeField] private SceneSO _LoginScene;

        [Header("Broadcasting on")] [SerializeField]
        private AssetReference _menuLoadChannel;
        
        private async UniTaskVoid Start()
        {
          await _managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
          var menuLoadChannel = await _menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>();
          menuLoadChannel.RaiseEvent(_LoginScene, true);
          SceneManager.UnloadSceneAsync(0);
        }
    }
}