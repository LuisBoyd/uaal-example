using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Systems.PersistantData
{
    public static class FileManager
    {
        public static bool WriteToFile(IPersistantData data)
        {
            //Check if file Exists First
            var fullpath = Path.Combine(Application.persistentDataPath, data.FileLocation);

            try
            {
                using (StreamWriter writer = File.CreateText(fullpath))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        jsonWriter.WriteStartObject();
                        data.Save(jsonWriter);
                        jsonWriter.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullpath} with exception {e}");
                return false;
            }

            return true;
        }
        
        public static async Task<bool> WriteToFileAsync(IPersistantData data)
        {
            //Check if file Exists First
            var fullpath = Path.Combine(Application.persistentDataPath, data.FileLocation);

            try
            {
                using (StreamWriter writer = File.CreateText(fullpath))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        await jsonWriter.WriteStartObjectAsync();
                        await data.SaveAsync(jsonWriter);
                        await jsonWriter.FlushAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullpath} with exception {e}");
                return false;
            }

            return true;
        }
        
        public static bool LoadFromFile(IPersistantData obj)
        {
            var fullpath = Path.Combine(Application.persistentDataPath, obj.FileLocation);
            if(!File.Exists(fullpath))
                File.WriteAllText(fullpath, "");

            try
            {
                using (StreamReader reader = File.OpenText(fullpath))
                {
                    using (JsonReader jsonReader = new JsonTextReader(reader))
                    {
                        jsonReader.DateFormatString = "dd/MM/yyyy HH:mm:ss";
                        //TODO later with different culture language.
                        obj.Load(jsonReader);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read from {fullpath} with exception {e}");
                obj.On_FailedLoad();
                return false;
            }

            return true;
        }
        
        public static async Task<bool> LoadFromFileAsync(IPersistantData obj)
        {
            var fullpath = Path.Combine(Application.persistentDataPath, obj.FileLocation);
            if(!File.Exists(fullpath))
                await File.WriteAllTextAsync(fullpath, "");

            try
            {
                using (StreamReader reader = File.OpenText(fullpath))
                {
                    using (JsonReader jsonReader = new JsonTextReader(reader))
                    {
                        jsonReader.DateFormatString = "dd/MM/yyyy HH:mm:ss";
                        //TODO later with different culture language.
                        await obj.LoadAsync(jsonReader);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read from {fullpath} with exception {e}");
                obj.On_FailedLoad();
                return false;
            }

            return true;
        }
        
        public static bool MoveFile(IPersistantData obj, string newFilename)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, obj.FileLocation);
            var newFullPath = Path.Combine(Application.persistentDataPath, newFilename);
            try
            {

                if (File.Exists(newFullPath))
                {
                    File.Delete(newFullPath);
                }

                if (!File.Exists(fullPath))
                {
                    return false;
                }
                
                File.Move(fullPath, newFullPath);
                obj.FileLocation = newFilename;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to move file from {fullPath} to {newFullPath} with exception {e}");
                return false;
            }

            return true;
        }
    }
}