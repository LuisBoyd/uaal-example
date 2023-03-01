using RCRCoreLib.Core.Systems.Tutorial.TutorialPredicates;

namespace RCRCoreLib.Core.Node.Nodes
{
    public abstract class TutorialNodeAction<T> : BaseTutorialNode where T : TutorialAction, new()
    {
        public T action;

        public override void Execute()
        {
            action = new T();
        }
        public override void Update()
        {
            if(!action.KeepWaiting())
                NextNode("exit");
        }
    }
}