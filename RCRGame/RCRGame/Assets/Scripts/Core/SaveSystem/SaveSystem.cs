using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace RCRCoreLib.Core.SaveSystem
{
    public static class SaveSystem
    {
        private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
        public const string FILE_NAME = "SaveFile";
        private const string SAVE_EXTENSTION = ".sav";
        public static string fileName { get; private set; }
        public static string filePath { get; private set; }

        public static void Initialize()
        {
            if (!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }

            fileName = FILE_NAME + SAVE_EXTENSTION;
            filePath = SAVE_FOLDER + FILE_NAME + SAVE_EXTENSTION;
        }

        public static void Save(SaveData saveObject)
        {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string saveString = JsonConvert.SerializeObject(saveObject, settings);
            Debug.Log($"Saved String {saveString}");
            File.WriteAllText(filePath, saveString);
        }

        public static SaveData Load()
        {
            if (File.Exists(filePath))
            {
                string saveString = File.ReadAllText(filePath);
                Debug.Log($"Loaded string {saveString}");
                SaveData loaded = JsonConvert.DeserializeObject<SaveData>(saveString);
                if (loaded == null)
                    return new SaveData();
                return loaded;
            }

            return new SaveData();
        }
    }
}