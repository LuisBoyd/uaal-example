using AI.behavior_tree;
using AI.Utilities;
using UnityEngine;
using XNode;

namespace AI.TaskLibary
{
    public abstract class BaseNode : Node
    {
        /// <summary>
        /// Called first time when this task is visited by it's parent
        /// </summary>
        public abstract void Initialize();

        protected abstract TaskStatus Process();
        
        /// <summary>
        /// Triggered every update tick.
        /// </summary>
        /// <returns>the status of the task</returns>
        public virtual TaskStatus Tick()
        {
            if(LastStatus == TaskStatus.Invalid)
                Initialize(); //If the node is being visited for the first time we Initialize it.
            var status = Process();
            if(status != TaskStatus.Running) //IF the Node returns  anything but running we can assume it's complete.
                OnComplete();
            LastStatus = status;
            return status;
        }
        
        /// <summary>
        /// Callback for when task is forcibly ended.
        /// </summary>
        public abstract void OnTerminate();
        
        /// <summary>
        /// On Completion of the task regardless of returned status.
        /// </summary>
        public abstract void OnComplete();
        

        /// <summary>
        /// Task name be used for display in the editor.
        /// </summary>
        public string TaskName { get; set; }
        
        /// <summary>
        /// Is the task enabled, disabled tasks are excluded in runtime.
        /// </summary>
        public bool Enabled { get; set; }
        
        /// <summary>
        /// The Status returned by the previous Update.
        /// </summary>
        public TaskStatus LastStatus { get; protected set; }

        [SerializeField, Input] public BT_Context inputResult;
    }
}