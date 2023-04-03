using UnityEngine;

namespace BehaviorTree.Nodes.DecoratorNode
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector]public Node child;
    }
}