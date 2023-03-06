using System.Collections.Generic;

namespace RCRCoreLib.Core.Optimisation.Patterns.Command
{
    public interface ICommandHandler<T> where T : Command
    {
        public LinkedList<T> CommandBuffer { get; }
        
        public LinkedListNode<T> Head { get; }

        void Record(T command);
        
        void Undo(int steps = 1);

        void Redo(int steps = 1);
    }
}