using C5;
using RCRMapLibrary.Comparers;

namespace RCRMapLibrary
{
    /// <summary>
    /// Chunks Only Store TileLayer Data
    /// </summary>
    public class Chunk
    {
        public const int ChunkWidth = 128;
        public const int ChunkHeight = 128;

        public IntervalHeap<TileLayer> TileLayers;

        public Chunk()
        {
            TileLayers = new IntervalHeap<TileLayer>(new TileLayerComparer());
        }
    }
}