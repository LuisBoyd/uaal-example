namespace RCRCoreLib.Core.AI.TaskSystem.Tasks
{
    public abstract class TaskBase
    {
        public TaskCancellationToken Token;

        protected TaskBase() => Token = new TaskCancellationToken(this);
    }
}