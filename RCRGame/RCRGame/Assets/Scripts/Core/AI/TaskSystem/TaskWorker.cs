using RCRCoreLib.Core.AI.TaskSystem.Tasks;
using UnityEngine;

namespace RCRCoreLib.Core.AI.TaskSystem
{
    public abstract class TaskWorker<T> : MonoBehaviour where T: TaskBase
    {
        private enum State
        {
            WaitingForNextTask,
            ExecutingTask
        }

        protected IEntity Entity;
        private TaskSystem<T> m_taskSystem;
        private TaskBase Memory;
        private T CurrentTask;
        private State m_State;
        private float WaitingTimer;
        

        public virtual void Initialize(IEntity entity, TaskSystem<T> taskSystem)
        {
            this.Entity = entity;
            this.m_taskSystem = taskSystem;
            m_State = State.WaitingForNextTask;
        }
        /// <summary>
        /// Give the task worker a Starting Task
        /// </summary>
        public virtual void Initialize(IEntity entity, TaskSystem<T> taskSystem, T startingTask)
        {
            this.Entity = entity;
            this.m_taskSystem = taskSystem;
            this.CurrentTask = startingTask;
            m_State = State.ExecutingTask;
        }
        

        public void Update()
        {
            switch (m_State)
            {
                case State.WaitingForNextTask:
                    if (Memory != null)
                    {
                        m_State = State.ExecutingTask;
                        FilterExecutionMethod(Memory as T);
                        Memory = null;
                    }
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

        protected virtual void FilterExecutionMethod(T task)
        {
            CurrentTask = task;
        }

        private void RequestNextTask()
        {
            T taskBase = m_taskSystem.RequestNextTask();
            if (taskBase == null)
            {
                m_State = State.ExecutingTask;
                return;
            }

            m_State = State.ExecutingTask;
            FilterExecutionMethod(taskBase);
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
    }
}