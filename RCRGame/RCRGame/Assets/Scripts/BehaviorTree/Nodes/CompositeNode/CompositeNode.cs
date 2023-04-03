using System.Collections.Generic;

namespace BehaviorTree.Nodes.CompositeNode
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();
    }
}