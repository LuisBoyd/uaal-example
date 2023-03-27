using AI.TaskLibary;
using UnityEngine;
using XNode;

namespace AI.Leaf
{
    public abstract class LeafNode : BaseNode
    {
        //Remember to add abort support
#if UNITY_EDITOR
        
        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            switch (from.direction)
            {
                case NodePort.IO.Output:
                    //Make sure that I am not Giving out more connections from output than there should be children.
                    if(from.ConnectionCount > 1 && from.node == this)
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
                    if(to.ConnectionCount > 1 && to.node == this)
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