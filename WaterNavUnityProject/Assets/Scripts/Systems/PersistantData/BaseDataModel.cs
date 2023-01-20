using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Systems.PersistantData
{
    public abstract class BaseDataModel : MonoBehaviour, IPersistantData
    {
        protected string _fileLocation = "";

        protected Dictionary<JsonToken, Action<JsonReader>> _JsonTokenlookup;

        protected IEnumerable<FieldInfo> fields;

        private FieldInfo _currentField;

        public virtual string FileLocation
        {
            get => _fileLocation;
            set => _fileLocation = value;
        }

        protected virtual void Awake()
        {
            fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance
                                                                              | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttributes(typeof(JsonRequiredAttribute), false).Length > 0);
            _fileLocation = "DefaultData.json";
            _JsonTokenlookup = new Dictionary<JsonToken, Action<JsonReader>>()
            {
                {JsonToken.None, JsonNone},
                {JsonToken.StartArray, JsonArray},
                {JsonToken.Boolean, JsonPrimitive},
                {JsonToken.Bytes, JsonPrimitive},
                {JsonToken.Date, JsonPrimitive},
                {JsonToken.String, JsonPrimitive},
                {JsonToken.Float, JsonPrimitive},
                {JsonToken.Integer, JsonPrimitive},
                {JsonToken.PropertyName, JsonProperty}
            };
        }
        public virtual void Save(JsonWriter writer)
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance
                                                                              | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fields)
            {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false);
                if (attributes.Length > 0)
                {
                    if (fieldInfo.FieldType.IsArray)
                    {
                        writer.WritePropertyName(fieldInfo.Name);
                        writer.WriteStartArray();
                        Array a = (Array) fieldInfo.GetValue(this);
                        for (int i = 0; i < a.Length; i++)
                            writer.WriteValue(a.GetValue(i));
                        writer.WriteEnd();
                    }
                    else if(fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string))
                    {
                        writer.WritePropertyName(fieldInfo.Name);
                        writer.WriteValue(fieldInfo.GetValue(this));
                    }
                    else
                    {
                        string jsonValue = JsonConvert.SerializeObject(fieldInfo.GetValue(this));
                        writer.WritePropertyNameAsync(fieldInfo.Name);
                        writer.WriteValueAsync(jsonValue);
                    }
                }
            }
        }
        public virtual void Load(JsonReader reader)
        {
            while (reader.Read())
            {
                if (_JsonTokenlookup.ContainsKey(reader.TokenType))
                {
                    _JsonTokenlookup[reader.TokenType](reader);
                }
            }
        }
        private bool CheckJSONtokenType(JsonToken token)
        {
            return token is JsonToken.Boolean or JsonToken.Bytes or JsonToken.Date
                or JsonToken.String or JsonToken.Float or JsonToken.Integer;

        }
        public virtual async Task LoadAsync(JsonReader reader)
        {
            while (await reader.ReadAsync())
            {
                // if (reader.Value != null)
                // {
                //     Debug.Log($"Token: {reader.TokenType}, Value: {reader.Value}");
                // }
                // else
                // {
                //     Debug.Log($"Token: {reader.TokenType}");
                // }
                if (_JsonTokenlookup.ContainsKey(reader.TokenType))
                {
                    _JsonTokenlookup[reader.TokenType](reader);
                }
            }
        }
        public virtual async Task SaveAsync(JsonWriter writer)
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance
                                                                              | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (FieldInfo fieldInfo in fields)
            {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(JsonRequiredAttribute), false);
                if (attributes.Length > 0)
                {
                    if (fieldInfo.FieldType.IsArray)
                    {
                        await writer.WritePropertyNameAsync(fieldInfo.Name);
                        await writer.WriteStartArrayAsync();
                        Array a = (Array) fieldInfo.GetValue(this);
                        for (int i = 0; i < a.Length; i++)
                            await writer.WriteValueAsync(JsonConvert.SerializeObject(a.GetValue(i)));
                        await writer.WriteEndAsync();
                    }
                    else if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string))
                    {
                        await writer.WritePropertyNameAsync(fieldInfo.Name);
                        await writer.WriteValueAsync(fieldInfo.GetValue(this));
                    }
                    else
                    {
                        string jsonValue = JsonConvert.SerializeObject(fieldInfo.GetValue(this));
                        await writer.WritePropertyNameAsync(fieldInfo.Name);
                        await writer.WriteValueAsync(jsonValue);
                    }
                }
            }
        }
        public virtual void On_FailedLoad()
        {
         
        }
        
        private void JsonNone(JsonReader obj)
        {
           
        }
        
        private void JsonArray(JsonReader reader)
        {
            List<string> readValues = new List<string>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                if (CheckJSONtokenType(reader.TokenType))
                {
                    readValues.Add((string)reader.Value);
                }
            }
            var json = JsonConvert.SerializeObject(readValues);
            var converted = JsonConvert.DeserializeObject(json, _currentField.FieldType);
            if (converted != null)
            {
                _currentField.SetValue(this, converted);
            }
        }

        private void JsonPrimitive(JsonReader reader)
        {
            if (reader.Value != null)
            {
                var converted = JsonConvert.DeserializeObject(reader.Value.ToString(), _currentField.FieldType);
                if(converted != null)
                    _currentField.SetValue(this, converted);
            }
        }

        private void JsonProperty(JsonReader reader)
        {
            _currentField = fields.First(x => x.Name == (string)reader.Value);
        }
    }
}