using System;
using Newtonsoft.Json;
using Sirenix.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utility
{
    public class JsonSerializer : ISerializer<string>
    {
        public string Serialize<T>(T obj) where T : class
        {
            string serializedJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            if (string.IsNullOrEmpty(serializedJson))
            {
                Debug.LogWarning($"Serialization for {typeof(T).GetNiceName()} failed");
                return string.Empty;
            }
            return serializedJson;
        }

#if UNITY_EDITOR
        public static string Serialize<T>(string defaultDestinationPath, T obj)
        {
            string destination = EditorUtility.SaveFilePanel("Save File As",
                defaultDestinationPath, $"New {typeof(T).GetNiceName()}",
                "json").TrimEnd('/');
            if (string.IsNullOrEmpty(destination))
            {
                Debug.LogWarning($"File could not be serialized {destination}");
                return string.Empty;
            }
            string serializedJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            if (string.IsNullOrEmpty(serializedJson))
            {
                Debug.LogWarning($"Serialization for {typeof(T).GetNiceName()} failed");
                return string.Empty;
            }
            if (!FileHelper.WriteToOrCreateFile(destination, serializedJson))
            {
                Debug.LogWarning($"Failed to write {typeof(T).GetNiceName()} " +
                                 $"to {destination}");
                return String.Empty;
            }
            return serializedJson;
        }
#endif
    }
}