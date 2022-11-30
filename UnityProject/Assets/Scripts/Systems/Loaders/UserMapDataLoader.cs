using System;
using System.Threading.Tasks;
using DataStructures;
using RCR.Interfaces;

namespace RCR.Systems.Loaders
{
    public class UserMapDataLoader : IObjectLoader<MapData>
    {
        public async Task<MapData> LoadObject(Uri phpRequest)
        {
            throw new NotImplementedException();
        }
    }
}