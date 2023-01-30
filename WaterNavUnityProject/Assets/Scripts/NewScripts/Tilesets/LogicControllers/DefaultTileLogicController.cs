namespace RCR.Settings.NewScripts.Tilesets.LogicControllers
{
    public class DefaultTileLogicController : LogicTileController
    {
        public const DefaultTileLogicController Default = default;


        public DefaultTileLogicController(LogicTileController controller) : base(controller)
        {
        }

        public override void Start( ITileInfo data)
        {
            base.Start(data);
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