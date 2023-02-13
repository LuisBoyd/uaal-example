using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RCRCoreLibrary;

namespace RCRMapLibrary.JsonConverters
{
    public class TileBaseConverter : JsonConverter<TileBase>
    {
        public override void WriteJson(JsonWriter writer, TileBase? value, JsonSerializer serializer)
        {
            if(value == null)
                return;
            
            JToken t = JToken.FromObject(value);
            t.WriteTo(writer);
        }

        public override TileBase? ReadJson(JsonReader reader, Type objectType, TileBase? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (!hasExistingValue)
                existingValue = Activator.CreateInstance<TileBase>();
            if (existingValue == null)
                return null;

            while (reader.Read())
            {
                switch (reader.Value)
                {
                    case nameof(existingValue.ID):
                        existingValue.ID = reader.ReadAsInt32() ?? -1;
                        break;
                    case nameof(existingValue.CustomProperties):
                        JArray CustomPropertyArray = JArray.Load(reader);
                        foreach (JToken token in CustomPropertyArray)
                        {
                            CustomProperty property = token.ToObject<CustomProperty>();
                            if (property != null)
                                existingValue.CustomProperties.Add(property);
                        }
                        break;
                }
            }

            return existingValue;
        }
    }
}