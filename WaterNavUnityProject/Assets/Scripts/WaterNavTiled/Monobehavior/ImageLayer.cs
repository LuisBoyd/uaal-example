using Newtonsoft.Json.Bson;
using UnityEngine;

namespace WaterNavTiled
{
    public class ImageLayer : Layer
    {
        [SerializeField]private string Image;
        [SerializeField] private bool repeatX;
        [SerializeField] private bool repeatY;
        [SerializeField] private string transparentColor;

        public override void GetObjectData(BsonDataWriter info)
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
            info.WritePropertyName("Image");
            info.WriteValue(Image);
            info.WritePropertyName("repeatX");
            info.WriteValue(repeatX);
            info.WritePropertyName("repeatY");
            info.WriteValue(repeatY);
            info.WritePropertyName("transparentColor");
            info.WriteValue(transparentColor);
            info.WriteEndObject();
        }

        public override void CollectMonoData(){}
       
    }
}