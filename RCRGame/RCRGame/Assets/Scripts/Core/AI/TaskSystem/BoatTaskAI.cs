using RCRCoreLib.Core.AI.TaskSystem.Tasks;
using UnityEngine;

namespace RCRCoreLib.Core.AI.TaskSystem
{
    public class BoatTaskAI : TaskWorker<BoatTask>
    {
        public override void Initialize(IEntity entity, TaskSystem<BoatTask> taskSystem, BoatTask startingTask)
        {
            base.Initialize(entity, taskSystem, startingTask);
            FilterExecutionMethod(startingTask);
        }

        protected override void FilterExecutionMethod(BoatTask task)
        {
            switch (task)
            {
                case BoatTask.BoatMoveToEnd boatMoveToEnd:
                    Execute_MoveToEndTask(boatMoveToEnd);
                    break;
            }
        }

        private void Execute_MoveToEndTask(BoatTask.BoatMoveToEnd task)
        {
            Debug.Log("Executing BoatMoveToEnd");
            Entity.MoveTo(task.endPoint, task.onReached);
            //TODO when The Boat Has Reached the end give it back the object Pool
        }
    }
}