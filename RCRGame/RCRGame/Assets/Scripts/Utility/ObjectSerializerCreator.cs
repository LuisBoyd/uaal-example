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
        public static void Serialize <T>(string defaultDestinationPath,T obj, Action<T> onSerializeCompleted = null) 
            where T : new()
        {
            var container = new ObjectSerializeContainer<T>(defaultDestinationPath, onSerializeCompleted);
            container.Serialize(obj);
        }

        private class ObjectSerializeContainer<T> where T : new()
        {
            private Action<T> onSerializeCompleted;
            private string defaultDestinationPath;

            public ObjectSerializeContainer(string defaultDestinationPath, Action<T> onSerializeCompleted = null)
            {
                this.onSerializeCompleted = onSerializeCompleted;
                this.defaultDestinationPath = defaultDestinationPath;
            }
            

            public void Serialize(T data)
            {
                string dest = this.defaultDestinationPath.TrimEnd('/');

                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                if (!string.IsNullOrEmpty(dest) && PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest))
                {
                   string databytes = SerializeJson(data);
                    bool written = WriteToFile(dest, databytes);
                    if(onSerializeCompleted != null && written)
                        onSerializeCompleted(data);
                }
            }

            private string SerializeByType(T data)
            {
                string content;
// #if UNITY_EDITOR
//                 switch (data)
//                 {
//                     case SerializedScriptableObject obj:
//                         if (AssetDatabase.Contains(obj))
//                             content = SerializedScriptableObject(obj);
//                         else
//                             content = SerializeJson(data);
//                         break;
//                     case ScriptableObject scriptableObject:
//                         if (AssetDatabase.Contains(scriptableObject))
//                             content = SerializeAsset(data);
//                         else
//                             content = SerializeJson(data);
//                         break;
//                     default:
//                         content = SerializeJson(data);
//                         break;
//                 } //If the object is an asset in the editor I need to serialize it a different way compared to an instanced object.
// #else
                content = SerializeJson(data);
//#endif
                return content;
            }
            
            private string SerializeJson(T data)
            {
                return JsonConvert.SerializeObject(data, Formatting.Indented);
            }

            private bool WriteToFile(string filePath,string bytes)
            {
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (var writer = new BinaryWriter(fs))
                        { 
                            writer.Write(bytes);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    return false;
                }
                return true;
            }
        }
    }
}