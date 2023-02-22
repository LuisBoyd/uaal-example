using RCRCoreLib.Core.AI.TaskSystem;
using RCRCoreLib.Core.AI.TaskSystem.Tasks;
using UnityEngine;

namespace RCRCoreLib.Core.Entity
{
    public class BoatSpawner : Spawner<BoatTask>
    {

        [SerializeField] 
        private Transform DeSpawnPoint;
        
        public override void Spawn(Vector3 location, Quaternion rotation)
        {
            Entity boatEntity = EntityObjectPool.Request();
            boatEntity.transform.position = location;
            boatEntity.transform.rotation = rotation;
            boatEntity.Initialize(EntityType);
            TaskWorker<BoatTask> aiTaskWorker = boatEntity.GetComponent<TaskWorker<BoatTask>>();
            if (GiveStartingTask)
            {
                aiTaskWorker.Initialize(boatEntity, TaskSystem,
                    new BoatTask.BoatMoveToEnd(DeSpawnPoint.position,
                        () => { EntityObjectPool.Return(boatEntity); }));
            }


        }
    }
}