using System;
using Newtonsoft.Json;

namespace WaterNavTiled.CustomConverters
{
    public class GroupLayerJsonConverter : JsonConverter<GroupLayer>
    {
        public override void WriteJson(JsonWriter writer, GroupLayer value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override GroupLayer ReadJson(JsonReader reader, Type objectType, GroupLayer existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}