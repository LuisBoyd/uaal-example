using System;
using System.Linq;
using Newtonsoft.Json;

namespace RCRCoreLibrary.JsonConverters
{
    public class CustomPropertyConverter : JsonConverter<CustomProperty>
    {
        public override void WriteJson(JsonWriter writer, CustomProperty? value, JsonSerializer serializer)
        {
            writer.WritePropertyName(nameof(value.Name));
            if (value != null)
            {
                writer.WriteValue(value.Name ?? null);

                writer.WritePropertyName(nameof(value.Type));
                writer.WriteValue(value.Type ?? null);

                writer.WritePropertyName(nameof(value.value));
                writer.WriteValue(value.value ?? null);
            }
        }

        public override CustomProperty? ReadJson(JsonReader reader, Type objectType, CustomProperty? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (!hasExistingValue)
                existingValue = Activator.CreateInstance<CustomProperty>();

            if (existingValue == null)
                return null;

            while (reader.Read())
            {
                switch (reader.Value)
                {
                    case nameof(existingValue.Name):
                        existingValue.Name = reader.ReadAsString() ?? string.Empty;
                        break;
                    case nameof(existingValue.Type):
                        existingValue.Type = reader.ReadAsString() ?? string.Empty;
                        break;
                    case nameof(existingValue.value):
                        existingValue.value = reader.ReadAsString() ?? string.Empty;
                        break;
                    default:
                        break;
                }
            }

            return existingValue;
        }
    }
}