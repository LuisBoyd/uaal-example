using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Systems.PersistantData
{
    public static class DataReader
    {
        private static string _destination;
        public static string Destination
        {
            get
            {
                if (string.IsNullOrEmpty(_destination))
                {
                    Debug.LogError($"Persistent Data location is not set {_destination}");
                    return null;
                }

                return _destination;
            }
            set
            {
                _destination = Path.Combine(Application.dataPath, value);
            }
        }

        // public static async Task<JObject> Read(string filename)
        // {
        //     Destination = filename;
        //     if(!File.Exists(Destination))
        //         return null;
        //
        //     using (StreamReader reader = File.OpenText(filename))
        //     {
        //         using (JsonTextReader Jreader = new JsonTextReader(reader))
        //         {
        //             while (Jreader.Read())
        //             {
        //                 if (Jreader.Value != null)
        //                 {
        //                     
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}