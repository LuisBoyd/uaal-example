using System;
using System.IO;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using SerializationUtility = UnityEditor.SerializationUtility;

namespace Utility
{
    public static class ObjectSerializerCreator
    {
        public static void SerializeSave <T>(string defaultDestinationPath,T obj, Action<T> onSerializeCompleted = null) 
            where T : new()
        {
            var container = new ObjectSerializeContainer<T>(defaultDestinationPath, onSerializeCompleted);
            container.SerializeSave(obj);
        }

        public static string Serialize<T>(T obj, Action<T> onSerializeCompleted = null)
        where T : new()
        {
            var container = new ObjectSerializeContainer<T>(onSerializeCompleted);
            return container.Serialize(obj);
        } 
        
#if UNITY_EDITOR
        
        public static void ShowDialog<T>(string defaultDestinationPath,T obj, Action<T> onSerializeCompleted = null) 
            where T : new()
        {
            
            string dest = EditorUtility.SaveFilePanel("Save File as", defaultDestinationPath, "New " + typeof(T).GetNiceName(), "json");
            if (!string.IsNullOrEmpty(dest))
            {
                var container = new ObjectSerializeContainer<T>(dest, onSerializeCompleted);
                container.Serialize(obj);
            }
        }
#endif

        private class ObjectSerializeContainer<T> where T : new()
        {
            private Action<T> onSerializeCompleted;
            private string defaultDestinationPath;

            public ObjectSerializeContainer(string defaultDestinationPath, Action<T> onSerializeCompleted = null)
            {
                this.onSerializeCompleted = onSerializeCompleted;
                this.defaultDestinationPath = defaultDestinationPath;
            }

            public ObjectSerializeContainer(Action<T> onSerializeCompleted = null)
            {
                this.onSerializeCompleted = onSerializeCompleted;
            }

            public void SerializeSave(T data)
            {
                string dest = this.defaultDestinationPath.TrimEnd('/');
                
                if (!string.IsNullOrEmpty(dest))
                {
                   string databytes = SerializeJson(data);
                   bool written = FileHelper.WriteToFile(dest, databytes);
                    if(onSerializeCompleted != null && written)
                        onSerializeCompleted(data);
                }
            }
            
            public string Serialize(T data)
            {
                string dest = this.defaultDestinationPath.TrimEnd('/');
                
                if (!string.IsNullOrEmpty(dest))
                {
                    string databytes = SerializeJson(data);
                    if(onSerializeCompleted != null)
                        onSerializeCompleted(data);
                    return databytes;
                }
                return string.Empty;
            }

            private string SerializeJson(T data)
            {
                return JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            
        }
    }
}