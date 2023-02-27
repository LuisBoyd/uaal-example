using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RCRCoreLib.Core.Systems.Unlockable;

namespace RCRCoreLib.Core.Systems
{
    [Serializable]
    public class UnlockData
    {
        public Dictionary<string, UnlockableStructure> LockedStructures
            = new Dictionary<string, UnlockableStructure>();

        public Dictionary<string, UnlockableBuilding> LockedBuildings =
            new Dictionary<string, UnlockableBuilding>();


        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            LockedStructures ??= new Dictionary<string, UnlockableStructure>();
            LockedBuildings ??= new Dictionary<string, UnlockableBuilding>();
        }
    }
}