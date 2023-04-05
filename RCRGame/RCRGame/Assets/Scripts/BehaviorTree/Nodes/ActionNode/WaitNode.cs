using UnityEngine;

namespace BehaviorTree.Nodes.ActionNode
{
    public class WaitNode : ActionNode
    {
        [SerializeField] 
        private float AwaitTime = 0.1f;

        private float TimeLeft;
        
        protected override void OnStart()
        {
            TimeLeft = AwaitTime;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                return State.Running;
            }
            return State.Success;
        }
    }
}