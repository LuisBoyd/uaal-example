using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes.CompositeNode
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector]public List<Node> children = new List<Node>();

        public override Node DeepCopy()
        {
            CompositeNode node = ScriptableObject.Instantiate(this);
            node.children = this.children.ConvertAll(c => c.DeepCopy());
            return node;
        }
    }
}