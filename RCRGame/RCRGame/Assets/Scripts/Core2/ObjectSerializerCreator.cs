using System;
using System.IO;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

namespace RCRCoreLib.Core
{
    public class ObjectSerializerCreator
    {
        public static void ShowDialog<T>(string defaultDestinationPath,T obj, Action<T> onSerializeCompleted = null) 
        where T : new()
        {
            var container = new ObjectSerializeContainer<T>(defaultDestinationPath, onSerializeCompleted);
            container.ShowSaveFileDialog(obj);
        }
        
        public static void Serialize <T>(string defaultDestinationPath,T obj, Action<T> onSerializeCompleted = null) 
            where T : new()
        {
            var container = new ObjectSerializeContainer<T>(defaultDestinationPath, onSerializeCompleted);
            container.BypassSerialize(obj);
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

            public void ShowSaveFileDialog(T data)
            {
                string dest = this.defaultDestinationPath.TrimEnd('/');

                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                    AssetDatabase.Refresh();
                }
                dest = EditorUtility.SaveFilePanel("Save File as", dest, "New " + typeof(T).GetNiceName(), "rbin");
                if (!string.IsNullOrEmpty(dest) && PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest))
                {
                    byte[] databytes = Serialize(data);
                    File.WriteAllBytes(dest, databytes);
                    if(onSerializeCompleted != null)
                        onSerializeCompleted(data);
                }
            }
            
            public void BypassSerialize(T data)
            {
                string dest = this.defaultDestinationPath.TrimEnd('/');

                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                if (!string.IsNullOrEmpty(dest) && PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest))
                {
                    byte[] databytes = Serialize(data);
                    bool written = WriteToFile(dest, databytes);
                    if(onSerializeCompleted != null && written)
                        onSerializeCompleted(data);
                }
            }

            private byte[] SerializeByType(T data)
            {
                byte[] bytes;
#if UNITY_EDITOR
                switch (data)
                {
                    case SerializedScriptableObject obj:
                        if (AssetDatabase.Contains(obj))
                            bytes = SerializedScriptableObject(obj);
                        else
                            bytes = Serialize(data);
                        break;
                    case ScriptableObject scriptableObject:
                        if (AssetDatabase.Contains(scriptableObject))
                            bytes = SerializeAsset(data);
                        else
                            bytes = Serialize(data);
                        break;
                    default:
                        bytes = Serialize(data);
                        break;
                } //If the object is an asset in the editor I need to serialize it a different way compared to an instanced object.
#else
                bytes = Serialize(data);
#endif
                return bytes;
            }
            

#if UNITY_EDITOR
            
            private byte[] SerializeAsset(T data)
            {
                return SerializationUtility.SerializeValue(data, DataFormat.Binary);
            }
            
            private byte[] SerializedScriptableObject(SerializedScriptableObject data)
            {
                return null;
            }
#endif
            private byte[] Serialize(T data)
            {
                return SerializationUtility.SerializeValue(data, DataFormat.Binary);
            }

            private bool WriteToFile(string filePath,byte[] bytes)
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