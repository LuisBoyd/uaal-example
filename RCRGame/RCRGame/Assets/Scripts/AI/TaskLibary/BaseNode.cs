using AI.behavior_tree;
using AI.Utilities;
using UnityEngine;
using XNode;

namespace AI.TaskLibary
{
    public abstract class BaseNode : Node
    {
        protected abstract TaskStatus Process();

        /// <summary>
        /// Triggered every update tick.
        /// </summary>
        /// <returns>the status of the task</returns>
        public virtual TaskStatus Tick()
        {
            if(LastStatus == TaskStatus.Invalid || LastStatus == TaskStatus.Success) //If it's success we re-initialize it because new loop
                Initialize(); //If the node is being visited for the first time we Initialize it every loop.
            var status = Process();
            if(status != TaskStatus.Running) //IF the Node returns  anything but running we can assume it's complete.
                OnComplete();
            LastStatus = status;
            return status;
        }

        /// <summary>
        /// Callback for when task is forcibly ended.
        /// </summary>
        public virtual void OnTerminate()
        {
            AbortFlag = false;
        }

        /// <summary>
        /// On Completion of the task regardless of returned status.
        /// </summary>
        public virtual void OnComplete()
        {
            //LastStatus = TaskStatus.Invalid;
        }
        
        /// <summary>
        /// Called first time when this task is visited by it's parent
        /// </summary>
        public virtual void Initialize(){}

        /// <summary>
        /// In the Need of termination the Node can be exited returns a failure result from the node.
        /// OnTerminate Called after.
        /// </summary>
        public virtual void Abort()
        {
            AbortFlag = true;
        }

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
        public TaskStatus LastStatus
        {
            get
            {
                return Status;
            }
            protected set
            {
                Status = value;
            }
        }
        [SerializeField] 
        protected TaskStatus Status; //TODO remove as not to give user access to directly modify the TaskStatus. debugging purposes
        
        /// <summary>
        /// Flagged if the current Node need's to be aborted flag is reset onTerminate.
        /// </summary>
        protected bool AbortFlag { get; set; }

        [SerializeField, Input] public BT_Context inputResult;

        public const string InputPortName = "inputResult";
        public const string OutputPortName = "outputResult";
    }
}