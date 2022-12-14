using System.Collections.Generic;
using System.IO;
using WaterNavTiled.Data;
using WaterNavTiled.Interfaces;

namespace WaterNavTiled
{
    public abstract class SerializationWriter<T> where T : ISerializable
    { 
        public abstract void Write(T serializable, bool AutoStartEnd = false);

        public abstract void Write(IEnumerable<T> serializables,bool AutoStartEnd = false);

    }
}