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

        public Dictionary<string, DelayedPlaceableObjectData> delayedPlaceableObjectDatas =
            new Dictionary<string, DelayedPlaceableObjectData>();

        public static string GeneratedID()
        {
            idCount++;
            return idCount.ToString();
        }

        public void AddData(Data data)
        {
            switch (data)
            {
                case PlaceableObjectData plobjData when plobjData.GetType() == typeof(PlaceableObjectData):
                    if (placeableObjectDatas.ContainsKey(plobjData.ID))
                        placeableObjectDatas[plobjData.ID] = plobjData;
                    else
                    {
                        placeableObjectDatas.Add(plobjData.ID, plobjData);
                    }
                    break;
                case DelayedPlaceableObjectData delplobjData when delplobjData.GetType() == typeof(DelayedPlaceableObjectData):
                    if (delayedPlaceableObjectDatas.ContainsKey(delplobjData.ID))
                        delayedPlaceableObjectDatas[delplobjData.ID] = delplobjData;
                    else
                    {
                        delayedPlaceableObjectDatas.Add(delplobjData.ID,delplobjData);
                    }
                    break;
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