using UnityEngine;

namespace BehaviorTree.Nodes.ActionNode
{
    public abstract class ActionNode : Node
    {
        public override Node DeepCopy()
        {
            return ScriptableObject.Instantiate(this);
        }
    }
}