using System;
using System.IO;
using Newtonsoft.Json;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Utility
{
    public static class ObjectDeserializerCreator
    {
        public static T Deserialize<T>(string defaultDestinationPath, Action<T>
            onDeserializedComplete) where T : new()
        {
            var container = new ObjectDeserializeContainer<T>(defaultDestinationPath,
                onDeserializedComplete);
            return container.Deserialize();
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


            public T Deserialize()
            {
                string dest = this.defaultDestinationPath.TrimEnd('/');
                
                if (!string.IsNullOrEmpty(dest) && File.Exists(dest))
                {
                    T DeserializedValue = Deserialize(dest);
                    if (onDeserializeCompleted != null && DeserializedValue != null)
                    {
                        onDeserializeCompleted(DeserializedValue);
                        return DeserializedValue;
                    }else if (DeserializedValue != null)
                        return DeserializedValue;
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
                        using (var sr = new StreamReader(fs))
                        {
                            // value = Sirenix.Serialization.SerializationUtility.DeserializeValue<T>(
                            //     reader.Stream, DataFormat.JSON);
                            value = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
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