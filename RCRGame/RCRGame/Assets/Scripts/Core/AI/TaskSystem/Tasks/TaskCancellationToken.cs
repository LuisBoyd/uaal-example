namespace RCRCoreLib.Core.AI.TaskSystem.Tasks
{
    public class TaskCancellationToken
    {
        public bool StopTask { get; private set; }
        private TaskBase refrencedTask;

        public TaskCancellationToken(TaskBase task)
        {
            refrencedTask = task;
        }

        public TaskBase CancelTask()
        {
            StopTask = true;
            return refrencedTask;
        }
    }
}