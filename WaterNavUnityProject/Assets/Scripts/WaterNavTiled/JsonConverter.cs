using WaterNavTiled.Data;
using WaterNavTiled.Interfaces;

namespace WaterNavTiled
{
    public class JsonConverter : SerializationConverter<IJsonSerializable>
    {
        public override void serialize(IJsonSerializable serializable)
        {
            JsonSerializationInfo info = new JsonSerializationInfo(serializable);
            serializable.GetObjectData(ref info);
            
        }
    }
}