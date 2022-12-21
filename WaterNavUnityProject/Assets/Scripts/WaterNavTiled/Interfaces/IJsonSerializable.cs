using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using WaterNavTiled.Data;

namespace WaterNavTiled.Interfaces
{
    public interface IJsonSerializable : ISerializable
    {
        public void GetObjectData(JsonWriter info);

        public void ReciveObjectData(JsonTextReader info);

    }
}