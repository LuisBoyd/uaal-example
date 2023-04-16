using System;
using Core3.MonoBehaviors;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Utility;

namespace Core.Services
{
    public class SceneLoader: BaseMonoBehavior
    {
        
        /*
         *  The scenes are split up like this
         * \- Initialization Scene
         *      +- AppPersistant manager Scene
         *      |   +-LoginScene
         *      |   +-ManagementHub Scene
         *      |   |   +-Persistant Gameplay Managers Scene
         *      |   |   +-Persistant VisitGameplay Managers Scene
         *      |   |   |   +- Gameplay scene
         *      |   |   |   +- Visit Scene
         *      |   |   |   +- Card Award Scene
         */
        
        [SerializeField] 
        private SceneSO _gameplayManagerSO; //this would be the Persistant Gameplay managers scene SO

#if UNITY_EDITOR
        public LoadEventChannelSO _coldStartupLoad = default;  //cold startup event is editor only for when we skip the initialization scene setup.
        public UnityAction<SceneSO,bool> _coldStartupAction;
#endif
        public LoadEventChannelSO _loadSceneEvent = default; //Changing Scene Event
        public UnityAction<SceneSO, bool> _loadSceneAction;
        public BoolEventChannelSO _toggleLoadingScreen = default;
        public EventRelay _OnSceneReady;

        private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
        private AsyncOperationHandle<SceneInstance> _persistantGameplayManagers;

        private SceneInstance _persistantGameplayManagerInstance = new SceneInstance();
        private bool _isLoading; //A lock to prevent multiple call's to loading at once.
        
        //Parameters that come from scene load requests
        private SceneSO _sceneToLoad;
        private SceneSO _currentlyLoadedScene;
        private bool _showLoadingScreen;

        protected  void Awake()
        {
#if UNITY_EDITOR
            _coldStartupAction = UniTaskHelper.UnityAction<SceneSO,bool>(On_ColdStartUp);
#endif
            _loadSceneAction = UniTaskHelper.UnityAction<SceneSO, bool>(On_SceneLoad);
        }

        private void OnEnable()
        {
            _loadSceneEvent.onEventRaised += _loadSceneAction;
#if UNITY_EDITOR
            _coldStartupLoad.onEventRaised += _coldStartupAction; //Done for UniTask System
#endif
        }
        
        private void OnDisable()
        {
            _loadSceneEvent.onEventRaised -= _loadSceneAction;
#if UNITY_EDITOR
            _coldStartupLoad.onEventRaised -= _coldStartupAction;
#endif
        }
        
        private async UniTaskVoid On_SceneLoad(SceneSO sceneToLoad, bool showLoadingScreen)
        {
            //Prevent double loading
            if(_isLoading) return;
        
            _sceneToLoad = sceneToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;
        
            //In case the Persistent gameplay is not loaded when we go into a gameplay scene
            if (sceneToLoad.sceneType == SceneSO.SceneType.Gameplay &&
                (_persistantGameplayManagerInstance.Scene == null || !_persistantGameplayManagerInstance.Scene.isLoaded))
            {
                _persistantGameplayManagers =
                    _gameplayManagerSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                await _persistantGameplayManagers;
                _persistantGameplayManagerInstance = _persistantGameplayManagers.Result;
            }

            var sceneInstance = await UnloadPreviousScene();
            OnNewSceneLoaded(sceneInstance);
            
        }
        
#if UNITY_EDITOR
        private async UniTaskVoid On_ColdStartUp(SceneSO currentlyOpenedScene, bool showLoadingScreen)
        {
            _currentlyLoadedScene = currentlyOpenedScene;
            if (_currentlyLoadedScene.sceneType == SceneSO.SceneType.Gameplay)
            {
                //The Persistant Managers/ App managers are already loaded by now.
                _persistantGameplayManagers = _gameplayManagerSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                await _persistantGameplayManagers;
                _persistantGameplayManagerInstance = _persistantGameplayManagers.Result;
               _OnSceneReady.RaiseEvent();
            }
        }
#endif
        private async UniTask<AsyncOperationHandle<SceneInstance>> UnloadPreviousScene()
        {
            if (_currentlyLoadedScene != null) //Will be null if the game was started in the Initialisation scene
            {
                if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
                {
                   await _currentlyLoadedScene.sceneReference.UnLoadScene(); //Unload the scene through its AssetReference, i.e. through the Addressable system
                }
#if UNITY_EDITOR
                else
                {
                    //Only used when, after a "cold start", the player moves to a new scene
                    //Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
                    //the scene needs to be unloaded using regular SceneManager instead of as an Addressable
                    await SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
                }
#endif
            }
            
            return await LoadNewScene();
        }

        private async UniTask<AsyncOperationHandle<SceneInstance>> LoadNewScene()
        {
            if (_showLoadingScreen)
            {
                _toggleLoadingScreen.RaiseEvent(true);
            }

            _loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            await _loadingOperationHandle;
            return _loadingOperationHandle;
        }

        private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            _currentlyLoadedScene = _sceneToLoad;
            Scene s = obj.Result.Scene;
            SceneManager.SetActiveScene(s);

            _isLoading = false;
            if(_showLoadingScreen)
                _toggleLoadingScreen.RaiseEvent(false);
            
            _OnSceneReady.RaiseEvent();
        }
    }
}