using System;
using System.Collections.Generic;
using AI.TaskLibary;
using XNode;

namespace AI.Composite
{
    public class SequenceNode : BaseCompositeNode
    {
        protected IEnumerator<NodePort> currentChild;
        

        protected override TaskStatus Process()
        {
            if (currentChild == null)
            {
                //Means there are no children return success??
                return TaskStatus.Success;
            }
            while (currentChild.MoveNext())
            {
                if (AbortFlag)
                {
                    OnTerminate();
                    return TaskStatus.Failed;
                }
                if (currentChild.Current != null && currentChild.Current.node is BaseNode)
                {
                    BaseNode connectedNode = currentChild.Current.node as BaseNode;
                    if (connectedNode != null)
                    {
                        TaskStatus status = connectedNode.Tick(); //Could be Null.
                        if (status != TaskStatus.Success)
                            return status;
                    }
                    else
                    {
                        throw new Exception("Connected Node is not of type BaseNode");
                    }
                }
            }
            return TaskStatus.Success; //We have Reached the end of the children node with none of them failing.
        }
        
        public override void OnTerminate()
        {
            base.OnTerminate();
            foreach (NodePort output in Outputs)
            {
                if (output.node != null && output.node is BaseNode)
                {
                    (output.node as BaseNode).Abort();
                }
            }
        }

        public override void Initialize()
        {
            children.Clear();
            NodePort outputPort = GetPort(OutputPortName);
            if (outputPort != null)
            {
                for (int i = 0; i < outputPort.ConnectionCount; i++)
                {
                    children.Add(outputPort.GetConnection(i));
                }
            }
            currentChild = children.GetEnumerator();
        }
        
    }
}