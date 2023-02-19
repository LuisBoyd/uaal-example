using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RCRCoreLib.Core.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public static int idCount;

        public Dictionary<string, PlaceableObjectData> placeableObjectDatas =
            new Dictionary<string, PlaceableObjectData>();

        public static string GeneratedID()
        {
            idCount++;
            return idCount.ToString();
        }

        public void AddData(Data data)
        {
            if (data is PlaceableObjectData plObjData)
            {
                if (placeableObjectDatas.ContainsKey(plObjData.ID))
                {
                    placeableObjectDatas[plObjData.ID] = plObjData;
                }
                else
                {
                    placeableObjectDatas.Add(plObjData.ID, plObjData);
                }
            }
        }

        public void RemoveData(Data data)
        {
            if (data is PlaceableObjectData plObjData)
            {
                if (placeableObjectDatas.ContainsKey(plObjData.ID))
                {
                    placeableObjectDatas.Remove(plObjData.ID);
                }
            }
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            placeableObjectDatas ??= new Dictionary<string, PlaceableObjectData>();
        }
        
    }
}