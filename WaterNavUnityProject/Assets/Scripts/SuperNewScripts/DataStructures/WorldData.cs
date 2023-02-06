using RCR.Settings.Collections;

namespace RCR.Settings.SuperNewScripts.DataStructures
{
    public struct WorldData
    {
        public AdjacencyMatrix<ChunkBlock> ChunkData { get; private set; }
    }
}