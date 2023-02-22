using System;
using RCRCoreLib.Core.AI.TaskSystem.Tasks;
using RCRCoreLib.Core.Entity;
using UnityEngine;

namespace RCRCoreLib.Core.AI.TaskSystem
{
    public interface IEntity
    {
        public Transform Transform { get; }

        void Initialize(EntityType typeData);

        public void InterruptTask(TaskBase task);

        void MoveTo(Vector3 position, Action onArrivedAtPosition = null);
    }
}