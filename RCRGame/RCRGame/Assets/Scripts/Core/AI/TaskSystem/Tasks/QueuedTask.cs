using System;

namespace RCRCoreLib.Core.AI.TaskSystem.Tasks
{
    public class QueuedTask<TTask> where TTask: TaskBase
    {
        private Func<TTask> m_tryGetFunc;

        public QueuedTask(Func<TTask> tryGetFunc)
        {
            this.m_tryGetFunc = tryGetFunc;
        }

        public TTask TryDequeueTask() => TryDequeueTask();
    }
}