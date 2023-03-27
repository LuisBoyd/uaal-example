using System.Collections.Generic;
using AI.TaskLibary;

namespace AI.Composite
{
    public class SequenceNode : BaseCompositeNode
    {
        protected IEnumerator<BaseNode> currentChild;

        public override void Initialize()
        {
            currentChild = children.GetEnumerator();
        }

        protected override TaskStatus Process()
        {
            while (true)
            {
                if (currentChild.Current != null)
                {
                    TaskStatus status = currentChild.Current!.Tick(); //Could be Null.
                    if (status != TaskStatus.Success)
                        return status;
                }

                if (!currentChild.MoveNext())
                    return TaskStatus.Success; //We have Reached the end of the children node with none of them failing.
            }
        }

        public override void OnTerminate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnComplete()
        {
            throw new System.NotImplementedException();
        }
    }
}