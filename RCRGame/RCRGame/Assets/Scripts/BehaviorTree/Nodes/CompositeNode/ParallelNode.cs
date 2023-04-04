using System;
using UnityEngine;

namespace BehaviorTree.Nodes.CompositeNode
{
    public class ParallelNode : CompositeNode
    {
        private enum parallelPolicy
        {
           RequireOne,
           RequireAll
        }
        [SerializeField] private parallelPolicy Policy;

        private int successCount;
        private int failCount;

        protected override void OnStart()
        {
            successCount = 0;
            failCount = 0;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            foreach (Node child in children)
            {
                State status = child.Update();
                switch (status)
                {
                    case State.Failure:
                        failCount++;
                        if (Policy == parallelPolicy.RequireOne)
                            return State.Failure;
                        break;
                    case State.Success:
                        successCount++;
                        if (Policy == parallelPolicy.RequireOne)
                            return State.Success;
                        break;
                    case State.Aborted:
                        break;
                   
                }
            }

            if (Policy == parallelPolicy.RequireAll && failCount == children.Count)
                return State.Failure;
            if (Policy == parallelPolicy.RequireAll && successCount == children.Count)
                return State.Success;
            return State.Running;
        }
    }
}