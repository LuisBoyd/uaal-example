using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WaterNavTiled.Data;

namespace WaterNavTiled.CustomConverters
{
    public class MapWrapperJsonConverter : JsonConverter<MapWrapper>
    {
        public override void WriteJson(JsonWriter writer, MapWrapper value, JsonSerializer serializer)
        {
            writer.WritePropertyName("Map");
            writer.WriteStartObject();
            
            writer.WritePropertyName("TileWidth");
            writer.WriteValue(value.X);
            writer.WritePropertyName("TileHeight");
            writer.WriteValue(value.Y);
            
            writer.WritePropertyName("TileSize");
            writer.WriteValue(value.TileWidth);
            
            writer.WritePropertyName("Layers");
            writer.WriteStartArray();
            foreach (Layer layer in value.Layers)
            {
                serializer.Serialize(writer, layer, layer.GetType());
            }
            writer.WriteEndArray();
            
            writer.WritePropertyName("TileSets");
            writer.WriteStartArray();
            writer.WriteEndArray();
            
            writer.WriteEndObject();
        }

        public override MapWrapper ReadJson(JsonReader reader, Type objectType, MapWrapper existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}