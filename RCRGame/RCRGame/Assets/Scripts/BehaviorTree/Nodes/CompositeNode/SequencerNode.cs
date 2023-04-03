using UnityEngine;

namespace BehaviorTree.Nodes.CompositeNode
{
    /// <summary>
    /// Executes children from 0 to n (n being the length of the child list) and returns success if all children succeed or failure if otherwise.
    /// </summary>
    public class SequencerNode : CompositeNode
    {
        private int current;

        protected override void OnStart()
        {
            current = 0;
        }
        protected override void OnStop(){}


        protected override State OnUpdate()
        {
            var child = children[current];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                    break;
                case State.Failure:
                    return State.Failure;
                    break;
                case State.Success:
                    current++;
                    break;
            }
            return current == children.Count ? State.Success : State.Running;
        }

        public override Node DeepCopy()
        {
            SequencerNode NewInstance = ApplyDefaults(ScriptableObject.CreateInstance<SequencerNode>());
            children.ForEach(n =>
            {
                Node copiedChild = n.DeepCopy();
                NewInstance.children.Add(copiedChild);
            });
            return NewInstance;
        }

        public override object Clone() => DeepCopy();
    }
}