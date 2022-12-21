using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;

namespace WaterNavTiled
{
    public class GroupLayer : Layer
    {
        [SerializeField] private Layer[] layers;

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
            
            info.WritePropertyName("Layers");
            info.WriteStartArray();

            foreach (Layer layer in layers)
            {
                layer.GetObjectData(info);
            }
            
            info.WriteEndArray();
            
            info.WriteEndObject();
        }

        public override void CollectMonoData()
        {
            List<Layer> listLayers = new List<Layer>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Layer l = transform.GetChild(i).GetComponent<Layer>();
                if (l is not GroupLayer && l != null)
                {
                    listLayers.Add(l);
                }
                else
                {
                    Debug.LogWarning("Currently can't put a GroupLayer inside of a GroupLayer this can break the system \n Attempted to Fix Error");
                }
            }

            layers = listLayers.ToArray();
            listLayers.Clear();
            foreach (Layer layer in layers)
            {
                layer.CollectMonoData();
            }
        }
    }
}