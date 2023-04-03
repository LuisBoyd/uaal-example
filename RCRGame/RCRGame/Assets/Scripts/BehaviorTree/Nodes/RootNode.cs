using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class RootNode : Node
    {
        public Node child;
        
        protected override void OnStart(){}

        protected override void OnStop(){}

        protected override State OnUpdate() => child.Update();
        public override Node DeepCopy()
        {
            RootNode node = ScriptableObject.Instantiate(this);
            node.child = this.child.DeepCopy();
            return node;
        }
    }
}