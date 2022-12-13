using WaterNavTiled.Data;

namespace WaterNavTiled.Interfaces
{
    public interface IJsonSerializable : ISerializable
    {
        public void GetObjectData(ref JsonSerializationInfo info);

    }
}