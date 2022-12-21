using System;
using Newtonsoft.Json;

namespace WaterNavTiled.CustomConverters
{
    public class ObjectLayerJsonConverter : JsonConverter<ObjectLayer>
    {
        public override void WriteJson(JsonWriter writer, ObjectLayer value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override ObjectLayer ReadJson(JsonReader reader, Type objectType, ObjectLayer existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}