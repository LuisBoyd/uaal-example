namespace RCR.Settings.NewScripts.TaskSystem
{
    public class CustomerTaskWorkerAI: TaskWorkerAI<CustomerTask>
    {
        public CustomerTaskWorkerAI(IEntity entity, TaskSystem<CustomerTask> taskSystem) : base(entity, taskSystem)
        {
        }

        protected override void FilterExecutionMethod(CustomerTask task)
        {
            throw new System.NotImplementedException();
        }
    }
}