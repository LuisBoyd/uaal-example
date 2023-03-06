namespace RCRCoreLib.Core.Optimisation.Patterns.Command
{
    public abstract class TileCommand : Command
    {
        public abstract void Execute(ITileCommandHandler handler);
        public abstract void Undo(ITileCommandHandler handler);
    }
}