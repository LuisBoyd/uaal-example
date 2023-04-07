using System;
using System.IO;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using SerializationUtility = UnityEditor.SerializationUtility;

namespace RCRCoreLib.Core
{
    public class ObjectDeserializerCreator
    {
        public static T ShowDialog<T>(string defaultDestinationPath, Action<T> onSerializeCompleted = null) 
            where T : new()
        {
            var container = new ObjectDeserializeContainer<T>(defaultDestinationPath, onSerializeCompleted);
            return container.ShowOpenFileDialog();
        }
        
        public static T Deserialize <T>(string defaultDestinationPath,T obj, Action<T> onSerializeCompleted = null) 
            where T : new()
        {
            var container = new ObjectDeserializeContainer<T>(defaultDestinationPath, onSerializeCompleted);
            return container.BypassDeserialize();
        }

        private class ObjectDeserializeContainer<T> where T : new()
        {
            private Action<T> onDeserializeCompleted;
            private string defaultDestinationPath;
            
            public ObjectDeserializeContainer(string defaultDestinationPath, Action<T> onDeserializeCompleted = null)
            {
                this.onDeserializeCompleted = onDeserializeCompleted;
                this.defaultDestinationPath = defaultDestinationPath;
            }
            
            public T ShowOpenFileDialog()
            {
                string dest = this.defaultDestinationPath.TrimEnd('/');

                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                    AssetDatabase.Refresh();
                }
                dest = EditorUtility.OpenFilePanel("Open File as", dest, "rbin");
                if (!string.IsNullOrEmpty(dest) && PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest))
                {
                    T DeserializedValue = Deserialize(dest);
                    if (onDeserializeCompleted != null && DeserializedValue != null)
                    {
                        onDeserializeCompleted(DeserializedValue);
                        return DeserializedValue;
                    }
                }
                return default;
            }
            
            public T BypassDeserialize()
            {
                string dest = this.defaultDestinationPath.TrimEnd('/');

                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                if (!string.IsNullOrEmpty(dest) && PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest))
                {
                    T DeserializedValue = Deserialize(dest);
                    if (onDeserializeCompleted != null && DeserializedValue != null)
                    {
                        onDeserializeCompleted(DeserializedValue);
                        return DeserializedValue;
                    }
                }
                return default;
            }

            private T Deserialize(string filepath)
            {
                T value = default;
                if (!ReadFromFile(filepath, out value))
                {
                    Debug.LogWarning($"Failed to serialize {value.GetType().Name}");
                }
                return value;
            }

            private bool ReadFromFile(string filePath, out T value)
            {
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var reader = new BinaryReader(fs))
                        {
                            value = Sirenix.Serialization.SerializationUtility.DeserializeValue<T>(
                                reader.BaseStream, DataFormat.Binary);
                        }
                    }
                }
                catch (Exception e)
                {
                    value = default;
                    Debug.LogError(e.Message);
                    return false;
                }
                return true;
            }
        }
    }
}