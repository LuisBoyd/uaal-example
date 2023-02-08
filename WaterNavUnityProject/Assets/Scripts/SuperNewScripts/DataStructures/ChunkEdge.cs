using QuikGraph;

namespace RCR.Settings.SuperNewScripts.DataStructures
{
    public class ChunkEdge : IEdge<ChunkBlock>
    {
        public ChunkBlock Source { get; set; }
        public ChunkBlock Target { get; set; }
        
        
    }
}