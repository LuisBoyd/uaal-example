using System.Collections.Generic;
using System.Linq;
using AI.TaskLibary;
using AI.Utilities;
using UnityEngine;
using XNode;

namespace AI.Composite
{
    public abstract class BaseCompositeNode : BaseNode
    {
        [Output] public BT_Context outputResult;
        

        protected IList<NodePort> children = new List<NodePort>();

        public int MaxChildren
        {
            get => maxChildren;
        }
        [SerializeField] 
        protected int maxChildren;

        protected BaseCompositeNode()
        {
            maxChildren = 1;
        }
        

        public override object GetValue(NodePort port)
        {
            return inputResult;
        }

#if UNITY_EDITOR
        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            // if (to.ConnectionCount > 1)
            // {
            //     to.Disconnect(from);
            //     Debug.Log($"Disconnected {from.node.name} from {to.node.name}");
            //     return;
            // }
            // if (from.ConnectionCount > MaxChildren)
            // {
            //     from.Disconnect(to);
            //     Debug.Log($"Disconnected {from.node.name} from {to.node.name}");
            //     return;
            // }
            switch (from.direction)
            {
                case NodePort.IO.Output:
                    //Make sure that I am not Giving out more connections from output than there should be children.
                    if(from.ConnectionCount > MaxChildren && from.node == this)
                        from.Disconnect(to); 
                    return;
                    break;
                case NodePort.IO.Input:
                    //Make sure there is only 1 input going into a node
                    if(from.ConnectionCount > 1 && from.node == this)
                        from.Disconnect(to);
                    return;
                    break;
                
            }
            switch (to.direction)
            {
                case NodePort.IO.Output:
                    //Make sure that I am not Giving out more connections from output than there should be children.
                    if(to.ConnectionCount > MaxChildren && to.node == this)
                        to.Disconnect(from); 
                    return;
                    break;
                case NodePort.IO.Input:
                    //Make sure there is only 1 input going into a node
                    if(to.ConnectionCount > 1 && to.node == this)
                        to.Disconnect(from);
                    return;
                    break;
            }
        }
#endif
    }
}