using WaterNavTiled.Interfaces;

namespace WaterNavTiled
{
    public abstract class SerializationReader<T> where T : ISerializable
    {
        public abstract void Read(T serializable);
    }
}