using AI.TaskLibary;
using UnityEngine;

namespace AI.Leaf
{
    public class DebugStringNode : LeafNode
    {
        [SerializeField] [TextArea] private string message;
        protected override TaskStatus Process()
        {
            Debug.Log(message);
            return TaskStatus.Success;
        }
    }
}