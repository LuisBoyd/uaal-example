using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Systems.PersistantData
{
    public static class DataWriter
    {
        private static Stack<KeyValuePair<PersistantDataType, JObject>> _jsonValues;
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

        public static void WriteTO(KeyValuePair<PersistantDataType, JObject> value)
        {
            _jsonValues.Push(value);
        }
        public static void WriteTO(PersistantDataType type, JObject value)
        {
            WriteTO(new KeyValuePair<PersistantDataType, JObject>(type,value));
        }
        public static void WriteTO(IEnumerable<KeyValuePair<PersistantDataType, JObject>> values)
        {
            foreach (var value in values)
            {
                WriteTO(value);
            }
        }

        public static async Task FlushData()
        {
            IList<JObject> _PlayerData = new List<JObject>();
            IList<JObject> _MapData = new List<JObject>();
            while (_jsonValues.Count != 0)
            {
                var popped = _jsonValues.Pop();
                switch (popped.Key)
                {
                    case PersistantDataType.Map:
                        _MapData.Add(popped.Value);
                        break;
                    case PersistantDataType.Player:
                        _PlayerData.Add(popped.Value);
                        break;
                }
            }
        }

        private static async Task WriteToFile(string filename)
        {
            Destination = filename;
            if(!File.Exists(Destination))
                return;
            
        }
        
    }
    
    //https://stackoverflow.com/questions/6637679/reflection-get-attribute-name-and-value-on-property
}