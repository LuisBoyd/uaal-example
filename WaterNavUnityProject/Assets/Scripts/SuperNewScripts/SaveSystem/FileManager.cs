using System.Collections.Generic;
using System;
using System.IO;
using RCR.Settings.SuperNewScripts.World;
using UnityEngine;

namespace RCR.Settings.SuperNewScripts.SaveSystem
{
    public static class FileManager
    {
        private const string WorldsDirectory = "Worlds/";
        private const string ChunksDirectory = "Chunks/";

        private static Dictionary<Type, string> DirectoryLookUp = new Dictionary<Type, string>()
        {
            {typeof(WorldLoader), WorldsDirectory},
            {typeof(ChunkBlock), ChunksDirectory}
        };

        public static string RequestDirectory(Type type)
        {
            if (!DirectoryLookUp.ContainsKey(type))
            {
                Debug.LogWarning($"{type.Name} is not contained in the dictionary");
                return string.Empty;
            }

#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, DirectoryLookUp[type]);
#elif UNITY_STANDALONE_WIN || UNITY_IOS || UNITY_ANDROID
               return Path.Combine(Application.persistentDataPath, DirectoryLookUp[type]);
#endif

        }
        
    }
}