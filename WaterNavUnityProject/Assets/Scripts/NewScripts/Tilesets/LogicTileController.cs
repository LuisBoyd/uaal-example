namespace RCR.Settings.NewScripts.Tilesets
{
    public abstract class LogicTileController
    {

        protected LogicTileController wrapee;
        protected ITileInfo Data;
        
        // Any method in this class should take in an interface that represents
        // like an entity or something maybe not all just 1 to process the entity

        protected LogicTileController(LogicTileController controller)
        {
            this.wrapee = controller;
        }

        // I should pass in a ItileData interface that has all the information
        //relative to what's on top of the tile

        public virtual void Start(ITileInfo data)
        {
            Data = data;
            if (wrapee != null)
            {
                wrapee.Start(data);
            }
        }

        public virtual void Process()
        {
            if (wrapee != null)
            {
                wrapee.Process();
            }
        }

        public virtual void End()
        {
            if (wrapee != null)
            {
                wrapee.End();
            }
        }
    }
}