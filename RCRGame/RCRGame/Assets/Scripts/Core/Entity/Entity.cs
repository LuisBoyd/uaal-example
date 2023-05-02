using System;
using System.Threading;
using Core.Animation;
using Core3.MonoBehaviors;
using Cysharp.Threading.Tasks;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Entity
{
    [RequireComponent(typeof(IsometricAnimatorController))]
    [RequireComponent(typeof(AIPath))]
    public abstract class Entity : BaseMonoBehavior, IEntity
    {
        public Transform Transform
        {
            get => transform;

        }
        protected IsometricAnimatorController _isometricAnimatorController;
        protected AIPath _aiPath;
        private Action _onArrival;
        private CancellationTokenSource _movementTokenSource;

        [Title("Entity Configurations", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] protected float movementSpeed;

        private void Awake()
        {
            _isometricAnimatorController = GetComponent<IsometricAnimatorController>();
            _aiPath = GetComponent<AIPath>();
        }
        
        public virtual async UniTaskVoid Moveto(Vector3 position, Action onArrival = null)
        {
            if(_movementTokenSource != null)
                _movementTokenSource.Cancel();
            _movementTokenSource = new CancellationTokenSource();
            _aiPath.destination = position;
            //Start search for a path to destination immediately
            _aiPath.SearchPath();
            await UniTask.WaitUntil(() => _aiPath.pathPending || !_aiPath.reachedEndOfPath, PlayerLoopTiming.Update,
                _movementTokenSource.Token);
            if(_movementTokenSource.Token.IsCancellationRequested)
                return;
            if (onArrival != null)
                onArrival();
        }
    }
}