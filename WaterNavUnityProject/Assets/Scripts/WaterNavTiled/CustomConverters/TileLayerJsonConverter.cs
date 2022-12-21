using System;
using Newtonsoft.Json;

namespace WaterNavTiled.CustomConverters
{
    public class TileLayerJsonConverter : JsonConverter<TileLayer>
    {
        public override void WriteJson(JsonWriter writer, TileLayer value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override TileLayer ReadJson(JsonReader reader, Type objectType, TileLayer existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}