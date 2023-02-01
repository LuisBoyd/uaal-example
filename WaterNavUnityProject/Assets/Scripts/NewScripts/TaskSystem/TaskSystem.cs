using System;
using System.Collections.Generic;
using RCR.Utilities;
using UnityEngine;

namespace RCR.Settings.NewScripts.TaskSystem
{
    public abstract class TaskBase
    {
        #region fields
        public TaskCancellationToken Token; //By Keeping a refrence to the task you can cancel the task by setting the token to false
        #endregion

        protected TaskBase() => Token = new TaskCancellationToken(this);
    }
    public class QueuedTask<TTask> where TTask : TaskBase
    {
        #region fields
        /// <summary>
        /// Validation Code for when the task can be completed 
        /// </summary>
        private Func<TTask> tryGetTaskFunc;
        #endregion
        #region Constructor

        public QueuedTask(Func<TTask> tryGetTaskFunc)
        {
            this.tryGetTaskFunc = tryGetTaskFunc;
        }
        #endregion

        #region Public Methods

        public TTask TryDequeueTask() => tryGetTaskFunc();

        #endregion
    }

    // public abstract class TaskSystem
    // {
    //     private List<TaskBase> tasklist;
    //     private List<QueuedTask<TaskBase>> QueuedTaskList;
    //
    //
    //     public abstract TaskBase RequestNextTask();
    //     public abstract void AddTask(TaskBase taskBase);
    //     public abstract void EnqueueTask(QueuedTask<TaskBase> queuedTask);
    //     public abstract void EnqueueTask(Func<TaskBase> tryGetTaskFunc);
    //     protected abstract void DequeueTasks();
    //
    // }
    
    public class TaskSystem<TTask> where TTask : TaskBase
    {

        #region fields

        private List<TTask> tasklist;
        private List<QueuedTask<TTask>> QueuedTaskList; //Any QueuedTask has to be validated before being dequeued

        #endregion

        #region Constructors

        public TaskSystem()
        {
            tasklist = new List<TTask>();
            QueuedTaskList = new List<QueuedTask<TTask>>();
            FunctionPeriodic.Create(DequeueTasks, .2f); //Every 200ms
        }

        #endregion

        #region Public Methods

        public  TTask RequestNextTask()
        {
            //Worker Requesting Task
            if (tasklist.Count <= 0)
                return null; //No tasks are available

            TTask taskBase = tasklist[0]; //Store reference to first task in list
            tasklist.RemoveAt(0);
            return taskBase; //Return the first task in the list
        }

        public void AddTask(TTask taskBase)
        {
            tasklist.Add(taskBase);
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

        #endregion

        #region private Methods
        protected  void DequeueTasks()
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
                    LBUtilities.TextPopupMouse("Task Dequeued");
                }
                else
                {
                    //Return Task is null, keep it queued
                }
            }
        }
        
        #endregion
    }
}