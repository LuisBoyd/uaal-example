using System.Collections.Generic;

namespace AI.TaskLibary
{
    public interface ITaskParent : ITask
    {
        /// <summary>
        /// List Of Children Tasks.
        /// </summary>
        IList<ITask> Children { get; }
        
        /// <summary>
        /// Maximum Children Allowed for this parent.
        /// </summary>
        int MaxChildrenCount { get; }

        /// <summary>
        /// Add a child to this node
        /// </summary>
        /// <param name="child">the child task</param>
        /// <returns>this task</returns>
        ITaskParent AddChild(ITask child);
    }
}