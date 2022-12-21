using System;
using Newtonsoft.Json;

namespace WaterNavTiled.CustomConverters
{
    public class ImageLayerJsonConverter: JsonConverter<ImageLayer>
    {
        public override void WriteJson(JsonWriter writer, ImageLayer value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override ImageLayer ReadJson(JsonReader reader, Type objectType, ImageLayer existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}