using WaterNavTiled.Interfaces;

namespace WaterNavTiled
{
    public class JSONReader : SerializationReader<IJsonSerializable>
    {
        public JSONReader(){}
        
        public override void Read(IJsonSerializable serializable)
        {
            throw new System.NotImplementedException();
        }
    }
}