namespace RCRMapLibrary
{
    public class Chunk
    {
        public const int ChunkWidth = 128;
        public const int ChunkHeight = 128;
        private TileBase[,] TileArray;

        public Chunk()
        {
            TileArray = new TileBase[ChunkWidth, ChunkHeight];
        }
    }
}