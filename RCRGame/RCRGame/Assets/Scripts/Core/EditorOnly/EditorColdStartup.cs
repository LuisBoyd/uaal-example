using System;
using Core3.MonoBehaviors;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Core.EditorOnly
{
    public class EditorColdStartup : BaseMonoBehavior
    {
#if UNITY_EDITOR
        [SerializeField] 
        private SceneSO _thisScene; //The scriptable Object that refreshes this scene
        [SerializeField] 
        private SceneSO _persitantManagerScene; //The Scriptable object that references the persistent manager scene

        [SerializeField] private AssetReference _notifyColdStartupChannel = default;

        private bool isColdStart = false;

        private void Awake()
        {
            if (!SceneManager.GetSceneByName(_persitantManagerScene.sceneReference.editorAsset.name).isLoaded)
            {
                isColdStart = true;
            }
        }

        private async UniTaskVoid Start()
        {
            if (isColdStart)
            {
               var persistantManagerSceneInstance = await _persitantManagerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
               var coldstartUpChannel = await _notifyColdStartupChannel.LoadAssetAsync<LoadEventChannelSO>();
               if(coldstartUpChannel != null)
                   coldstartUpChannel.RaiseEvent(_thisScene, false);
            }
        }
        
        //May want to raise a fake event later for gameplay.
#endif
    }
}