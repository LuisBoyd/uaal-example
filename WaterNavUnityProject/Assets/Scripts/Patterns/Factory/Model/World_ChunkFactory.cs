using RCR.Settings.Map;
using UnityEngine;

namespace Patterns.Factory.Model
{
    public class World_ChunkFactory: Factory<WorldChunk>
    {
        [SerializeField] 
        private WorldChunk[] WorldChunks;
        public override WorldChunk Create()
        {
            return Instantiate(WorldChunks[Random.Range(0, WorldChunks.Length)]);
        }

        public override WorldChunk Clone(WorldChunk original)
        {
            throw new System.NotImplementedException();
        }
    }
}