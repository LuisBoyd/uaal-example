using UnityEngine;

namespace BehaviorTree.Nodes.DecoratorNode
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector]public Node child;

        public override Node DeepCopy()
        {
            DecoratorNode node = ScriptableObject.Instantiate(this);
            node.child = this.child.DeepCopy();
            return node;
        }
    }
}