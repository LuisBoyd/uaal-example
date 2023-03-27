using System;
using System.Linq;
using AI.TaskLibary;
using UnityEngine;

namespace AI.Decorator
{
    public class RepeatXNode : DecoratorNode
    {
        [SerializeField] 
        private int repeatTimes;
        
        protected override TaskStatus Process()
        {
            if (AbortFlag)
            {
                OnTerminate();
                return TaskStatus.Failed;
            }
            TaskStatus overAllStatus = 0;
            child = GetOutputPort(OutputPortName).Connection;
            if (child != null && child.node != null)
            {
                if (child.node is BaseNode)
                {
                    BaseNode connectedNode = child.node as BaseNode;
                    for (int i = 0; i < repeatTimes; i++)
                    {
                        overAllStatus = connectedNode.Tick();
                    }
                }
                else
                {
                    throw new Exception("Connected Node is not of type BaseNode");
                }
            }

            if (overAllStatus == TaskStatus.Invalid)
                overAllStatus = TaskStatus.Success; //this would only happen if the decorator has no child node.
            
            return overAllStatus;
        }
    }
}