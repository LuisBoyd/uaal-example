using System;
using RCR.Settings.NewScripts.TaskSystem;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public abstract class Entity<T> : MonoBehaviour, IEntity where T : TaskBase
    {
        protected TaskSystem<T> TaskSystem;
        protected TaskWorkerAI<T> WorkerAI;
        public Transform Transform { get; }

        public virtual void Setup(TaskSystem<T> taskSystem)
        {
            TaskSystem = taskSystem;
        }

        public void InterruptTask(TaskBase task)
        {
            if(task.GetType().IsAssignableFrom(typeof(T)))
                WorkerAI.Interrupt(task as T);
        }

        public abstract void MoveTo(Vector3 position, Action onArrivedAtPosition = null);

        protected virtual void Update()
        {
            WorkerAI.Update();
        }
    }
}