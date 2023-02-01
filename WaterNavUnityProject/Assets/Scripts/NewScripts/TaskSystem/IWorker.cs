using System;
using UnityEngine;

namespace RCR.Settings.NewScripts.TaskSystem
{
    public interface IEntity
    {
        public Transform Transform { get; }

        public void InterruptTask(TaskBase task);

        void MoveTo(Vector3 position, Action onArrivedAtPosition = null);
    }
}