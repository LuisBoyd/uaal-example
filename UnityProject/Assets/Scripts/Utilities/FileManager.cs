using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

namespace RCR.Utilities
{
    public static class FileManager
    {
        private const string m_MapPartitionManifest = "/Manifests/MapPartitionManifest.json";
        public static readonly string MapPartitionManifest;

        static FileManager()
        {
            MapPartitionManifest = Application.dataPath + m_MapPartitionManifest;
        }
        
        public static bool WriteToFile(string fileName, string fileContents, bool Fullpath = false)
        {
#if UNITY_EDITOR
            var fullPath = Path.Combine(Application.dataPath, fileName);
#elif UNITY_STANDALONE_WIN || UNITY_ANDROID || UNITY_IOS
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);
#endif
            if (Fullpath)
            {
                fullPath = fileName;
            }

            try
            {
                File.WriteAllText(fullPath, fileContents);
                return true;
            }catch(Exception e)
            {
                Debug.LogError($"Failed To Wirte To {fullPath} with exception {e}");
                return false;
            }
        }



        public static bool LoadFromFile(string fileName, out string result, bool Fullpath = false)
        {
#if UNITY_EDITOR
            var fullPath = Path.Combine(Application.dataPath, fileName);
#elif UNITY_STANDALONE_WIN || UNITY_ANDROID || UNITY_IOS
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);
#endif

            if (Fullpath)
            {
                fullPath = fileName;
            }

            if (!File.Exists(fullPath))
            {
                File.WriteAllText(fullPath, "");
            }
            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }catch(Exception e)
            {
                Debug.LogError($"Failed to Read from {fullPath} with exception {e}");
                result = "";
                return false;
            }
        }

        public static bool MoveFile(string filename, string newfilename)
        {

#if UNITY_EDITOR
            var fullPath = Path.Combine(Application.dataPath, filename);
            var newfullPath = Path.Combine(Application.dataPath, newfilename);
#elif UNITY_STANDALONE_WIN || UNITY_ANDROID  || UNITY_IOS
            var fullPath = Path.Combine(Application.persistentDataPath, filename);
            var newfullPath = Path.Combine(Application.persistentDataPath, newfilename);
#endif

            try
            {
                if (!File.Exists(newfullPath))
                {
                    File.Delete(newfullPath);
                }
                if (!File.Exists(fullPath))
                {
                    return false;
                }

                File.Move(fullPath, newfullPath);
            }catch(Exception e)
            {
                Debug.LogError($"Failed to Move File from {fullPath} to {newfullPath} with excepetion {e}");
                return false;
            }
            return true;
        }
    }
}
