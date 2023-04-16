using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Utility.Serialization
{
    public class ColorHandler : JsonConverter
    {
        public ColorHandler(){}

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            string val = ColorUtility.ToHtmlStringRGB((Color)value);
            writer.WriteValue(val);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            try
            {
                ColorUtility.TryParseHtmlString("#" + reader.Value, out Color loadedColor);
                return loadedColor;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to parse color {objectType} : {ex.Message}");
                return null;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}