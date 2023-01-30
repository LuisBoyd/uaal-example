using UnityEngine;

namespace RCR.Settings.NewScripts.Tilesets.LogicControllers
{
    public class DebuggerTileLogicController: LogicTileController
    {
        public DebuggerTileLogicController(LogicTileController controller) : base(controller)
        {
        }

        public override void Start( ITileInfo data)
        {
            base.Start(data);
            Debug.Log($"Spawner Tile {Data.WorldLocation}");
        }

        public override void Process()
        {
            base.Process();
        }

        public override void End()
        {
            base.End();
            Debug.Log($"END Spawner Tile {Data.WorldLocation}");
        }
    }
}