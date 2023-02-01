namespace RCR.Settings.NewScripts.TaskSystem
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