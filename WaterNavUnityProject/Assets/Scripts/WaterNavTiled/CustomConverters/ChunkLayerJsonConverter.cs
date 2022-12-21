using System;
using Newtonsoft.Json;

namespace WaterNavTiled.CustomConverters
{
    public class ChunkLayerJsonConverter : JsonConverter<ChunkLayer>
    {
        public override void WriteJson(JsonWriter writer, ChunkLayer value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override ChunkLayer ReadJson(JsonReader reader, Type objectType, ChunkLayer existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}