using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes.CompositeNode
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector]public List<Node> children = new List<Node>();
        
    }
}