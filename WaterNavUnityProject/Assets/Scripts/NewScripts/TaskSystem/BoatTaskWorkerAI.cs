namespace RCR.Settings.NewScripts.TaskSystem
{
    public class BoatTaskWorkerAI: TaskWorkerAI<BoatTask>
    {
        public BoatTaskWorkerAI(IEntity entity, TaskSystem<BoatTask> taskSystem) : base(entity, taskSystem)
        {
        }

        protected override void FilterExecutionMethod(BoatTask task)
        {
            throw new System.NotImplementedException();
        }
        
    }
}