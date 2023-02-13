using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RCRMapLibrary.JsonConverters
{
    public class TileTypeConverter : JsonConverter<TileType>
    {
        public override void WriteJson(JsonWriter writer, TileType value, JsonSerializer serializer)
        {
            if(value == null)
                return;
            
            JToken t = JToken.FromObject(value);
            if(t.Type != JTokenType.Object)
                t.WriteTo(writer);
            else
            {
                JObject o = (JObject) t;
                o.WriteTo(writer);
            }
        }

        public override TileType ReadJson(JsonReader reader, Type objectType, TileType existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (!hasExistingValue)
                existingValue = Activator.CreateInstance<TileType>();

            while (reader.Read())
            {
                switch (reader.Value)
                {
                    case nameof(existingValue.Name):
                        existingValue.Name = reader.ReadAsString() ?? null;
                        break;
                    case nameof(existingValue.Source):
                        existingValue.Source = reader.ReadAsString() ?? null;
                        break;
                    default:
                        break;
                }
            }

            return existingValue;
        }
    }
}