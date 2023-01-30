using RCR.Settings.NewScripts.Tilesets.LogicControllers;

namespace RCR.Settings.NewScripts.Tilesets
{
    public class PureLogicTile
    {
        public LogicDecorations LogicDecorations;
        public LogicTileController Controller;

        public PureLogicTile(LogicTile tile)
        {
            LogicDecorations = tile.LogicDecorations;
            AddLogicControllers();
        }
        
        private void AddLogicControllers()
        {
            Controller = DefaultTileLogicController.Default;
            if ((LogicDecorations & LogicDecorations.Debugger) != 0)
                Controller = new DebuggerTileLogicController(Controller);
            if ((LogicDecorations & LogicDecorations.Path) != 0)
                Controller = new PathTileLogicController(Controller);
            if ((LogicDecorations & LogicDecorations.Spawner) != 0)
                Controller = new SpawnerTileLogicController(Controller);
        }
    }
}