using RCR.Settings.NewScripts.Tilesets.LogicControllers;
using UnityEngine;

namespace RCR.Settings.NewScripts.Tilesets
{
    public class PureLogicTile
    {
        public LogicDecorations LogicDecorations;
        public Vector2Int location;
        //public LogicTileController Controller;

        public PureLogicTile(LogicTile tile, Vector2Int pos)
        {
            LogicDecorations = tile.LogicDecorations;
            location = pos;
            //AddLogicControllers();
        }
        
        private void AddLogicControllers()
        {
            // Controller = DefaultTileLogicController.Default;
            // if ((LogicDecorations & LogicDecorations.Debugger) != 0)
            //     Controller = new DebuggerTileLogicController(Controller);
            // if ((LogicDecorations & LogicDecorations.Path) != 0)
            //     Controller = new PathTileLogicController(Controller);
            // if ((LogicDecorations & LogicDecorations.Spawner) != 0)
            //     Controller = new SpawnerTileLogicController(Controller);
        }
    }
}