using UnityEngine;

namespace RCR.Settings.NewScripts.Tilesets.LogicControllers
{
    public class PathTileLogicController: LogicTileController
    {
        public PathTileLogicController(LogicTileController controller) : base(controller)
        {
        }

        public override void Start( ITileInfo data)
        {
            base.Start(data);
            Debug.Log($"Path Tile {Data.WorldLocation}");
        }

        public override void Process()
        {
            base.Process();
        }

        public override void End()
        {
           base.End();
        }
    }
}