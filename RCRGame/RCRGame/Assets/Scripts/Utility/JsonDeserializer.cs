#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using Newtonsoft.Json;
using Sirenix.Utilities;
using UnityEngine;

namespace Utility
{
    public class JsonDeserializer : IDeserializer<string>
    {
        public T Deserialize<T>(string data) where T : class
        {
            if (string.IsNullOrEmpty(data))
            {
                Debug.LogWarning($"Json is empty");
                return default;
            }
            T returnValue = JsonConvert.DeserializeObject<T>(data);
            if (returnValue == null)
            {
                Debug.LogWarning($"Deserialization encountered an error {typeof(T).GetNiceName()}");
                return default;
            }
            return returnValue;
        }

        public T DeserializeFromPath<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"Could not find file {filePath}");
                return default;
            }
            string serializedString = FileHelper.ReadAll(filePath);
            return Deserialize<T>(serializedString);
        }
        
#if UNITY_EDITOR
        public static T Deserialize<T>() where T : class
        {
            string filepath = EditorUtility.OpenFilePanel("Save File As",
                Application.dataPath, "json");
            if (string.IsNullOrEmpty(filepath))
            {
                Debug.LogWarning($"Loading the file encountered an error {typeof(T).GetNiceName()}");
                return default;
            }
            string SerializedString = FileHelper.ReadAll(filepath);
            if (string.IsNullOrEmpty(SerializedString))
            {
                Debug.LogWarning($"Reading the file encountered an error {typeof(T).GetNiceName()}");
                return default;
            }
            T returnValue = JsonConvert.DeserializeObject<T>(SerializedString);
            if (returnValue == null)
            {
                Debug.LogWarning($"Deserialization encountered an error {typeof(T).GetNiceName()}");
                return default;
            }
            return returnValue;
        }
#endif
    }
}