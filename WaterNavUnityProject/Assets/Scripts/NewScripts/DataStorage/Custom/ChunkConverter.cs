using System;
using System.Reflection;
using NewScripts.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RCR.Settings.NewScripts.DataStorage.Custom
{
    public class ChunkConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jo = Activator.CreateInstance<JObject>();
            var type = value.GetType();
            foreach (var fields in type.GetFields())
            {
                object[] attributes = fields.GetCustomAttributes(typeof(JsonRequiredAttribute), false);
                if(fields.FieldType.IsArray)
                    WriteArray(fields, writer);
                else
                {
                    writer.WritePropertyName(fields.Name);
                    writer.WriteValue(JsonConvert.SerializeObject(
                        fields.GetValue(this)));
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        // public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        // {
        //     // Chunk c = Activator.CreateInstance<Chunk>();
        //     // JObject jo = JObject.Load(reader);
        //     // return c;
        // }

        public override bool CanRead
        {
            get => false;
        }

        public override bool CanWrite
        {
            get => CanWrite;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsAssignableFrom(typeof(Chunk)))
                return true;
            return false;
        }

        private void WriteArray(FieldInfo FI, JsonWriter writer)
        {
            writer.WritePropertyName(FI.Name);
            writer.WriteStartArray();
            Array a = (Array) FI.GetValue(this);
            for (int i = 0; i < a.Length; i++)
                writer.WriteValue(a.GetValue(i));
            writer.WriteEnd();
        }
    }
}