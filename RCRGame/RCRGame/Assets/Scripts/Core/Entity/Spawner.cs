using System;
using System.Threading;
using Core.Optimisation.Patterns.Factory;
using Core.Optimisation.Patterns.ObjectPooling;
using Core3.MonoBehaviors;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Entity
{
    [RequireComponent(typeof(ComponentPool<Entity>))]
    [RequireComponent(typeof(Factory<Entity>))]
    public abstract class Spawner<T> : BaseMonoBehavior where T : Entity
    {
        [Title("Configurations", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] protected int StartUpPool = 5;
        [SerializeField] protected bool TimedSpawner; //Disable this for full control of when things spawn.
        [SerializeField] protected float SpawnRate = 10.0f;

        public bool EnableSpanwerTimed
        {
            get
            {
                return TimedSpawner;
            }
            set
            {
                if (!value)
                {
                    _timedSpawnertknSource.Cancel();
                }
                else
                {
                    if (_timedSpawnertknSource == null)
                        _timedSpawnertknSource = new CancellationTokenSource();
                    if (_timedSpawnertknSource.IsCancellationRequested)
                        _timedSpawnertknSource = new CancellationTokenSource();
                    
                }
            }
        }

        private CancellationTokenSource _timedSpawnertknSource;
        protected ComponentPool<Entity> EntityObjectPool;

        protected virtual void Awake()
        {
            EntityObjectPool = GetComponent<ComponentPool<Entity>>();
            EntityObjectPool.Factory = GetComponent<Factory<Entity>>();
            _timedSpawnertknSource = new CancellationTokenSource();
        }

        protected virtual void Start()
        {
            EntityObjectPool.Prewarm(StartUpPool);
            if (EnableSpanwerTimed)
            {
                SpawnTimer(_timedSpawnertknSource.Token).Forget();
            }
        }

        protected virtual void OnDisable()
        {
            _timedSpawnertknSource.Cancel();
        }

        protected virtual void SpawnEntity(Vector3 location, Quaternion rotation)
        {
            Debug.Log("Spawn");
        }

        protected virtual async UniTaskVoid SpawnTimer(CancellationToken tkn)
        {
            while (!tkn.IsCancellationRequested)
            {
                SpawnEntity(transform.position, Quaternion.identity);
                await UniTask.Delay(TimeSpan.FromSeconds(SpawnRate), DelayType.Realtime, PlayerLoopTiming.Update,
                    tkn);
            }
        }
    }
}