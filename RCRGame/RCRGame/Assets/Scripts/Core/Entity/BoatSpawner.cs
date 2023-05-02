using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

namespace Core.Entity
{
    public class BoatSpawner : Spawner<NarrowBoat>
    {
        [Title("Boat Spawner Configurations")]
        [Required] [SerializeField] protected PolygonCollider2D _outsideWorldPolygon;

        protected override void SpawnEntity(Vector3 location, Quaternion rotation)
        {
            NarrowBoat boat = EntityObjectPool.Request() as NarrowBoat;
            boat.transform.position = location;
            boat.transform.rotation = rotation;
        }

        protected override async UniTaskVoid SpawnTimer(CancellationToken tkn)
        {
            while (!tkn.IsCancellationRequested)
            {
                SpawnEntity(_outsideWorldPolygon.GeneratePointInsidePolygon(), Quaternion.identity);
                await UniTask.Delay(TimeSpan.FromSeconds(SpawnRate), DelayType.Realtime, PlayerLoopTiming.Update,
                    tkn);
            }
        }
    }
}