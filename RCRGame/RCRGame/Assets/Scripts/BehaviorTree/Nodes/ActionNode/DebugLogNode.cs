using UnityEngine;

namespace BehaviorTree.Nodes.ActionNode
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        
        protected override void OnStart()
        {
           Debug.Log($"OnStart {message}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop {message}");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate {message}");
            return State.Success;
        }

        public override Node DeepCopy()
        {
            DebugLogNode newInstance = ApplyDefaults(ScriptableObject.CreateInstance<DebugLogNode>());
           
            newInstance.message = new string(message);
            return newInstance;
        }

        public override object Clone() => DeepCopy();
    }
}