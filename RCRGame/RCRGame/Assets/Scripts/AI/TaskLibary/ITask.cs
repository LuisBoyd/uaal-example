using System.Collections.Generic;
using AI.behavior_tree;

namespace AI.TaskLibary
{
    public interface ITask
    {
        /// <summary>
        /// Called first time when this task is visited by it's parent
        /// </summary>
        void Initialize();

        /// <summary>
        /// Triggered every update tick.
        /// </summary>
        /// <returns>the status of the task</returns>
        TaskStatus Process();

        /// <summary>
        /// Callback for when task is forcibly ended.
        /// </summary>
        void OnTerminate();

        /// <summary>
        /// On Completion of the task regardless of returned status.
        /// </summary>
        void OnComplete();
        
        /// <summary>
        /// Task name be used for display in the editor.
        /// </summary>
        string TaskName { get; set; }
        
        /// <summary>
        /// Is the task enabled, disabled tasks are excluded in runtime.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// The Status returned by the previous Update.
        /// </summary>
        TaskStatus LastStatus { get; }
        
        /// <summary>
        /// Tree the node belongs to.
        /// </summary>
        IBehaviorTree ParentTree { get; set; }
    }
}