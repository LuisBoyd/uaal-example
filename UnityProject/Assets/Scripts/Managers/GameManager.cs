using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DataStructures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RCR.BaseClasses;
using RCR.DataStructures;
using RCR.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Information To be passed into me
 *
 * Location - poiID
 * user - username/Email 
 */

namespace RCR.Managers
{
    /// <summary>
    /// Main "Program Class" First Execution Happens here
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class GameManager: Singelton<GameManager>
    {
        private PlayerData m_playerData;
        private MapData m_mapData;

        private bool m_mapProccessingProblem = false;
        
        protected override void Awake()
        {
            base.Awake();
        }

        public void PoulateData(string userKey, string mapID, string regionName)
        {
            m_playerData = new PlayerData();
            m_mapData = new MapData();

            m_mapData.Region = regionName;
            m_mapData.MapID = mapID;
            m_playerData.UserKey = userKey;
        }

        public void ProcessAreaData(int[] areaData)
        {
            bool newArea = true;
            for (int i = 0; i < areaData.Length; i++)
            {
                if (areaData[i] != 0)
                    newArea = false;
            }

            m_mapData.MapIdArray = areaData;
            
            if (newArea)
            {
                ProcessNewArea();
            }
            else
            {
                ProcessOldAreaData();
            }
        }

        private void ProcessNewArea()
        {
            Action<bool, string> responseMethod = (b, s) =>
            {
                if (b)
                {
                    JObject RandomMapID = JObject.Parse(s);
                    if (RandomMapID != null)
                    {
                        m_mapData.MapIdArray[0] = RandomMapID["MapId"].ToObject<int>();
                        ProcessOldAreaData();
                    }
                    else
                    {
                        Debug.LogError("Problem with Processing New map");
                        MapProcessingProblem();
                    }
                }
            };

            StartCoroutine(NetworkManager.Instance.PutRequest("ProcessNewMap",
                new Dictionary<string, string>
                {
                    {"Userkey", m_playerData.UserKey},
                    {"Region", m_mapData.Region},
                    {"MapID", m_mapData.MapID}
                }, responseMethod));
        }

        private void ProcessOldAreaData()
        {
            LoggingUtils.Instance.Debug($"This is the map Data {m_mapData.MapIdArray.ToString()}");
        }

        private void MapProcessingProblem()
        {
            LoggingUtils.Instance.Debug("There was a processing problem");
        }
        
    }
}