namespace RCRMapLibrary
{
    public abstract class TileLayerT <T> : Layer where T: TileBase
    {
        public const int LayerWidth = 128;
        public const int LayerHeight = 128;
        public T[,] TileArray;

        protected TileLayerT()
        {
            TileArray = new T[LayerWidth, LayerHeight];
        }
    }
}