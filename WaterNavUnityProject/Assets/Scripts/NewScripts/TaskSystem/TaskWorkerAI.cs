using System;
using RCR.Utilities;
using UnityEngine;

namespace RCR.Settings.NewScripts.TaskSystem
{
    public abstract class TaskWorkerAI<T> where T : TaskBase
    {
        
        
        private enum State
        {
            WaitingForNextTask,
            ExecutingTask
        }
        
        #region fields

        private IEntity entity;
        private TaskSystem<T> TaskSystem;
        private TaskBase Memory;
        private T CurrentTask;
        private State state;
        private float WaitingTimer;
        #endregion
        
        #region Public Methods

        protected TaskWorkerAI(IEntity entity, TaskSystem<T> taskSystem)
        {
            this.entity = entity;
            this.TaskSystem = taskSystem;
            state = State.WaitingForNextTask;
        }
        
        public void Update()
        {
            switch (state)
            {
                case State.WaitingForNextTask:
                    
                    //If We had A task interrupt us go back to what we were originally doing
                    if (Memory != null)
                    {
                        state = State.ExecutingTask;
                        FilterExecutionMethod(Memory as T);
                        Memory = null;
                    }
                    //Waiting to request a new Task
                    WaitingTimer -= Time.deltaTime;
                    if (WaitingTimer <= 0)
                    {
                        float waitingTimerMax = .2f; //200ms
                        WaitingTimer = waitingTimerMax;
                        RequestNextTask();   
                    }
                    break;
                case State.ExecutingTask:
                    break;
            }
        }

        public virtual void Interrupt(T newTask)
        {
            Memory = CurrentTask.Token.CancelTask();
            FilterExecutionMethod(newTask);
        }
        
        #endregion

        #region Private Methods
        
        private void RequestNextTask()
        {
            LBUtilities.TextPopupMouse("RequestNextTask", new Vector3(0,7));
            T taskBase = TaskSystem.RequestNextTask();
            if (taskBase == null)
            {
                state = State.WaitingForNextTask;
                return;
            }

            state = State.ExecutingTask;
            FilterExecutionMethod(taskBase);
        }

        protected virtual void FilterExecutionMethod(T task)
        {
            CurrentTask = task;
        }

        //EXAMPLE ON HOW TO FILTER TASKS

        // protected void FilterExecutionMethod(T taskBase)
        // {
        //     switch (taskBase)
        //     {
        //         case Task.MoveToPosition moveToPositionTask:
        //             ExecuteTaskMoveToPosition(moveToPositionTask);
        //             break;
        //         case Task.PlayAnimation playAnimationTask:
        //             ExecuteTaskPlayAnimation(playAnimationTask);
        //             break;
        //         default:
        //             Debug.LogError($"{taskBase.GetType().Name} Is not assignable from Task");
        //             break;
        //     }
        // }

        //EXAMPLES OF EXECUTIONS

        // private void ExecuteTaskMoveToPosition(Task.MoveToPosition MoveToPositionTask)
        // {
        //     LBUtilities.TextPopupMouse("ExecuteTask MovePosition");
        //     //Probably around here make sure the target position is the point of thew A* system
        //     worker.MoveTo(MoveToPositionTask.targetPosition, () =>
        //     {
        //         state = State.WaitingForNextTask;
        //     }); 
        // }
        //
        // private void ExecuteTaskPlayAnimation(Task.PlayAnimation PlayAnimationTask)
        // {
        //     LBUtilities.TextPopupMouse("ExecuteTask MovePosition");
        //     //Worker PlayAnimation
        // }

        //TODO If I want to chain together Tasks Inside the delegate for on completing a task add the next function to do and so forth...

        #endregion
    }
}