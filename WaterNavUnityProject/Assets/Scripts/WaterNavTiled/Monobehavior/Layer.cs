using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaterNavTiled.Data;
using WaterNavTiled.Interfaces;

namespace WaterNavTiled
{
    
    public abstract class Layer : MonoBehaviour, IJsonSerializable
    {
        [SerializeField]protected string Classname;
        [SerializeField]protected int Id; //Readonly??
        [SerializeField]protected bool Locked;
        [SerializeField]protected string Name;
        [SerializeField] protected float Opacity;
        //Array of properties??
        [SerializeField] protected string Type; //Could replace with an enum for Tilelayer, ObjectGrouplayer, ImageLayer or just group
        [SerializeField] protected bool Visible;
        public virtual void GetObjectData(JsonWriter info)
        {
            info.WritePropertyName("Layer");
            info.WriteStartObject();
            info.WritePropertyName("Classname");
            info.WriteValue(Classname);
            info.WritePropertyName("Id");
            info.WriteValue(Id);
            info.WritePropertyName("Locked");
            info.WriteValue(Locked);
            info.WritePropertyName("Name");
            info.WriteValue(Name);
            info.WritePropertyName("Opacity");
            info.WriteValue(Opacity);
            info.WritePropertyName("Type");
            info.WriteValue(Type);
            info.WritePropertyName("Visible");
            info.WriteValue(Visible);
            info.WriteEndObject();
        }

        public void ReciveObjectData(JsonTextReader info)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ReciveObjectData(BsonDataReader info)
        {
            throw new System.NotImplementedException();
        }

        public abstract void CollectMonoData();
    }
}