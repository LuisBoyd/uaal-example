using System;
using System.Threading;
using System.Timers;
using RCRCoreLib.Core.AI.TaskSystem;
using RCRCoreLib.Core.AI.TaskSystem.Tasks;
using RCRCoreLib.Core.Optimisation.Patterns.Factory;
using RCRCoreLib.Core.Optimisation.Patterns.ObjectPooling;
using RCRCoreLib.Core.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace RCRCoreLib.Core.Entity
{
    [RequireComponent(typeof(ComponentPool<Entity>))]
    [RequireComponent(typeof(Factory<Entity>))]
    public abstract class Spawner<T> : MonoBehaviour where T : TaskBase
    {
        [SerializeField] 
        protected EntityType EntityType;

        [SerializeField] 
        protected int StartUpPool = 5;

        [SerializeField] 
        protected bool TimedSpawner; //Disable this for full control of when things spawn.
        

        [SerializeField] 
        protected bool GiveStartingTask;

        protected TaskSystem<T> TaskSystem;

        public bool EnableSpawnerTimed
        {
            get
            {
                return TimedSpawner;
            }
            set
            {
                if(!value)
                    PeriodicSpawn.DestroySelf();
                else
                {
                    if (PeriodicSpawn == null)
                    {
                        PeriodicSpawn = FunctionPeriodic.Create(SpawnTimerOnElapsed, () =>
                            {
                                return !EnableSpawnerTimed; //The Return Value Must be False in order for it not to be destroyed
                            }, SpawnRate); // * 1000 Millisecond To Second conversion
                        //TODO FIX with any PathFinding On StartUp Because of the order.
                    }
                }
            }
        }
        
        [SerializeField] 
        protected float SpawnRate = 10.0f;
        protected FunctionPeriodic PeriodicSpawn;


        protected ComponentPool<Entity> EntityObjectPool;

        protected virtual void Awake()
        {
            TaskSystem = new TaskSystem<T>();
            EntityObjectPool = GetComponent<ComponentPool<Entity>>();
            EntityObjectPool.Factory = GetComponent<Factory<Entity>>();
        }
        protected virtual void Start()
        { 
            EntityObjectPool.PreWarm(StartUpPool);
            if (EnableSpawnerTimed)
            {
                PeriodicSpawn = FunctionPeriodic.Create(SpawnTimerOnElapsed, () =>
                {
                    return !EnableSpawnerTimed; //The Return Value Must be False in order for it not to be destroyed
                }, SpawnRate); // * 1000 Millisecond To Second conversion
            }
        }


        public virtual void Spawn(Vector3 location, Quaternion rotation)
        {
            Debug.Log("Spawn");
        }


        protected virtual void SpawnTimerOnElapsed()
        {
            Debug.Log("SpawnTimerElapsed");
            Spawn(transform.position, Quaternion.identity);
        }
        
    }
}