using WaterNavTiled.Interfaces;

namespace WaterNavTiled
{
    public abstract class SerializationConverter<T> where T : ISerializable
    {
        public abstract void serialize(T serializable);//TODO possibly pass in a stream of some sort if need be
    }
}