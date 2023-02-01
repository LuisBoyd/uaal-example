using System;
using RCR.Settings.NewScripts.TaskSystem;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public class Customer: Entity<CustomerTask>
    {
        public override void Setup(TaskSystem<CustomerTask> taskSystem)
        {
            base.Setup(taskSystem);
            WorkerAI = new CustomerTaskWorkerAI(this, TaskSystem);
        }

        public override void MoveTo(Vector3 position, Action onArrivedAtPosition = null)
        {
            throw new NotImplementedException();
        }
    }
}