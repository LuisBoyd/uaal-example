namespace RCRMapLibrary
{
    public class TileLayer : Layer
    {
        private TileBase[,] TileArray;

        public TileLayer()
        {
            TileArray = new TileBase[LayerWidth, LayerHeight];
        }
        
        
    }
}