using AI.TaskLibary;
using XNode;

namespace AI.Decorator
{
    public abstract class DecoratorNode : BaseNode
    {
        protected BaseNode child;

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
        }
    }
}