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
            RootNode NewInstance = ApplyDefaults(ScriptableObject.CreateInstance<RootNode>());
            Node child = this.child.DeepCopy();
            NewInstance.child = child;
            return NewInstance;
        }

        public override object Clone() => DeepCopy();
    }
}