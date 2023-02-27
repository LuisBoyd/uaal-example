using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RCRCoreLib.Core.SaveSystem;
using UnityEngine;

namespace RCRCoreLib.Core.Systems.Unlockable
{
    public static class UnlockSystem 
    {
        public const string FILE_NAME = "UnlockFile";
        private const string SAVE_EXTENSTION = ".json";
        private static readonly string SAVE_FOLDER = Application.dataPath + "/Unlocks/";

        public static UnlockData UnlockData { get; private set; }

        public static CardData OwnedCardData;
        
        public static string fileName { get; private set; }
        public static string filePath { get; private set; }

        static UnlockSystem()
        {
            if (!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }

            fileName = FILE_NAME + SAVE_EXTENSTION;
            filePath = SAVE_FOLDER + FILE_NAME + SAVE_EXTENSTION;
        }
        
        public static bool LoadUnlocks()
        {
            if (File.Exists(filePath))
            {
                string saveString = File.ReadAllText(filePath);
                Debug.Log($"Loaded UnlockSystem {saveString}");
                UnlockData = JsonConvert.DeserializeObject<UnlockData>(saveString);
                if (UnlockData == null)
                    return false;
                return true;
            }
            return false;
        }

        public static IEnumerable<UnlockableBuilding> GetUnlockableBuildings()
            => UnlockData.LockedBuildings.Values;

        public static IEnumerable<UnlockableStructure> GetUnlockableStructures()
            => UnlockData.LockedStructures.Values;

    }
}