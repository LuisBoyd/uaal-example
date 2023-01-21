using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Systems.PersistantData
{
    public abstract class BaseDataModel : MonoBehaviour, IPersistantData
    {
        protected string _fileLocation = "";

        protected Dictionary<JsonToken, Action<JsonReader>> _JsonTokenlookup;

        protected IEnumerable<FieldInfo> fields;

        private FieldInfo _currentField;

        protected JsonSerializer _serializer;

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
            if (JsonConvert.DefaultSettings != null) 
                _serializer = JsonSerializer.Create(JsonConvert.DefaultSettings());
            if(_serializer == null)
                Debug.LogError($"serializer was not created so loading and saving can't occur");
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
        
        public virtual async Task LoadAsync(JsonReader reader)
        {
            JToken token = _serializer.Deserialize<JToken>(reader);
            foreach (FieldInfo fieldInfo in fields)
            {
                if(token[fieldInfo.Name] != null)
                    fieldInfo.SetValue(this,token[fieldInfo.Name].ToObject(fieldInfo.FieldType));
            }
        }
        public virtual async Task SaveAsync(JsonWriter writer)
        {
            foreach (FieldInfo fieldInfo in fields)
            {
                await writer.WritePropertyNameAsync(fieldInfo.Name);
                _serializer.Serialize(writer, fieldInfo.GetValue(this), fieldInfo.FieldType);
            }
        }
        public virtual void On_FailedLoad()
        {
         
        }
    }
}