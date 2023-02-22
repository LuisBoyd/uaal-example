using System;
using System.Collections.Generic;
using RCRCoreLib.Core.AI.TaskSystem.Tasks;

namespace RCRCoreLib.Core.AI.TaskSystem
{
    public class TaskSystem<TTask> where TTask : TaskBase
    {
        private List<TTask> taskList;
        private List<QueuedTask<TTask>> QueuedTaskList;

        public TaskSystem()
        {
            taskList = new List<TTask>();
            QueuedTaskList = new List<QueuedTask<TTask>>();
            //TODO have some method to periodacally Deque the QueuedTasks
        }

        public TTask RequestNextTask()
        {
            if (taskList.Count <= 0)
                return null;

            TTask taskBase = taskList[0];
            return taskBase;
        }

        public void AddTask(TTask taskbase)
        {
            taskList.Add(taskbase);
        }

        public void EnqueueTask(QueuedTask<TTask> queuedTask)
        {
            QueuedTaskList.Add(queuedTask);
        }

        public void EnqueueTask(Func<TTask> tryGetTaskFunc)
        {
            QueuedTask<TTask> queuedTask = new QueuedTask<TTask>(tryGetTaskFunc);
            QueuedTaskList.Add(queuedTask);
        }

        protected void DequeueTasks()
        {
            for (int i = 0; i < QueuedTaskList.Count; i++)
            {
                QueuedTask<TTask> queuedTask = QueuedTaskList[i];
                TTask taskBase = queuedTask.TryDequeueTask();
                if (taskBase != null)
                {
                    //Task Dequeued successfully, Add to normal List.
                    AddTask(taskBase);
                    QueuedTaskList.RemoveAt(i);
                    i--;
                }
                else
                {
                    //Return Task is null, keep it queued
                }
            }
        }
    }
}