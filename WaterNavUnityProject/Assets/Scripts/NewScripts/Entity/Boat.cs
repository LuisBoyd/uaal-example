using System;
using RCR.Settings.NewScripts.TaskSystem;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public class Boat : Entity<BoatTask>
    {
        public override void Setup(TaskSystem<BoatTask> taskSystem)
        {
            base.Setup(taskSystem);
            WorkerAI = new BoatTaskWorkerAI(this, TaskSystem);
        }

        public override void MoveTo(Vector3 position, Action onArrivedAtPosition = null)
        {
            throw new NotImplementedException();
        }
    }
}