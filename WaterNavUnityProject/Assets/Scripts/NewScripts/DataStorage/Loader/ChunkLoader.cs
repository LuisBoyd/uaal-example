using System;
using System.IO;
using System.Threading.Tasks;
using NewScripts.Model;
using RCR.Settings.NewScripts.DataStorage.Interfaces;

namespace RCR.Settings.NewScripts.DataStorage.Loader
{
    public class ChunkLoader : ILoader<Chunk>
    {
        public IFileReader FileReader { get; }

        public ChunkLoader(IFileReader reader) => FileReader = reader;
        
        public async Task<Chunk> ReconstructObject()
        {
            Chunk c = Activator.CreateInstance<Chunk>();
            return null;
        }
    }
}