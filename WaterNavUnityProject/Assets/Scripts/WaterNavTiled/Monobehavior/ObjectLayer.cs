using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;

namespace WaterNavTiled
{
    public class ObjectLayer : Layer
    {
        //Array of Objects
       [SerializeField] private string DrawOrder; //Could be represented with enum

       public override void GetObjectData(JsonWriter info)
       {
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
           info.WritePropertyName("DrawOrder");
           info.WriteValue(DrawOrder);
           info.WriteEndObject();
       }

       public override void CollectMonoData()
       {
           //TODO implement collecting Object Data later on
       }
    }
}