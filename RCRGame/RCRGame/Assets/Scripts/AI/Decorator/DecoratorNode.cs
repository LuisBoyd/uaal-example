using AI.TaskLibary;
using AI.Utilities;
using UnityEngine;
using XNode;

namespace AI.Decorator
{
    public abstract class DecoratorNode : BaseNode
    {
        private int MaxChildren
        {
            get => 1;
        }

        protected NodePort child;
        
        [Output] public BT_Context outputResult;

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

        public override void OnTerminate()
        {
            base.OnTerminate();
            if (child.node != null && child.node is BaseNode)
            {
                (child.node as BaseNode).Abort();
            }
        }

        public override object GetValue(NodePort port)
        {
            return inputResult;
        }
    }
}