using System;
using Core3.MonoBehaviors;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utility;

namespace Core.Services.Gameplay
{
    public class MarinaVisitService : BaseMonoBehavior
    {
        [SerializeField] private SceneSO _GameplayPersistantManagerScene;
        [SerializeField] private SceneSO _gamePlayScene;
        
        [Header("listening to")]
        [SerializeField]private IntEventChannelSO VisitMarinaEventChannel;

        [Header("Broadcasting On")] 
        [SerializeField]private LoadEventChannelSO _SceneLoadChannel;
        

        private UnityAction<int> OnVisitMarinaAction;

        private void Awake()
        {
            OnVisitMarinaAction = UniTaskHelper.UnityAction<int>(OnVisitMarina);
        }

        private void OnEnable()
        {
            VisitMarinaEventChannel.onEventRaised += OnVisitMarinaAction;
        }

        private void OnDisable()
        {
            VisitMarinaEventChannel.onEventRaised -= OnVisitMarinaAction;
        }

        private async UniTask OnVisitMarina(int marinaID)
        {
            
        } 

    }
}