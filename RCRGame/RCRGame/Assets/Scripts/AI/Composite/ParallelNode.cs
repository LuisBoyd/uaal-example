using System.Linq;
using AI.TaskLibary;
using UnityEngine;
using XNode;

namespace AI.Composite
{
    public class ParallelNode : BaseCompositeNode
    {
        enum ParallelPolicy
        {
            RequireOne,
            RequireAll,
        }

        [SerializeField] private ParallelPolicy Policy;

        private int childrenCount;
        
        private int successCount = 0;
        private int failCount = 0;

        // [SerializeField]
        // [Tooltip("How many child nodes must fail for this node to fail *Requires Policy to be CustomRequirement*")]
        // private int FailCount;
        //
        // [SerializeField]
        // [Tooltip(
        //     "How many child nodes must succeed for this node to succeed *Requires Policy to be CustomRequirement*")]
        // private int succeedCount;


        
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
                childrenCount = children.Count;
            }
            successCount = 0;
            failCount = 0;
        }

        protected override TaskStatus Process()
        {
            if (AbortFlag)
            {
                OnTerminate();
                return TaskStatus.Failed;
            }
            foreach (NodePort output in children)
            {
                if (output != null && output.node is BaseNode)
                {
                    BaseNode connectedNode = output.node as BaseNode;
                    if (connectedNode != null)
                    {
                        TaskStatus status = connectedNode.Tick(); //return the result no matter as its a OR Gate.

                        switch (status)
                        {
                            case TaskStatus.Failed:
                                failCount++;
                                if (Policy == ParallelPolicy.RequireOne)
                                    return TaskStatus.Failed;
                                break;
                            case TaskStatus.Success:
                                successCount++;
                                if (Policy == ParallelPolicy.RequireOne)
                                    return TaskStatus.Success;
                                break;
                        }
                    }
                }
                else
                {
                    failCount++;
                    if (Policy == ParallelPolicy.RequireOne)
                        return TaskStatus.Failed;
                }
            }

            if (Policy == ParallelPolicy.RequireAll && failCount == childrenCount)
                return TaskStatus.Failed;
            if (Policy == ParallelPolicy.RequireAll && successCount == childrenCount)
                return TaskStatus.Success;
            return TaskStatus.Running;
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
    }
}