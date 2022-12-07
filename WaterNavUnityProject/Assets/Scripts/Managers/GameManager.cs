using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataStructures;
using Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RCR.BaseClasses;
using RCR.DataStructures;
using RCR.Enums;
using RCR.Systems;
using RCR.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Grid = UnityEngine.Grid;

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
        private string m_userKey;
        private string m_region;
        private int m_mapID;
        public string UserKey
        {
            get => m_userKey;
        }
        public string Region
        {
            get => m_region;
        }
        public int MapID
        {
            get => m_mapID;
        }

        private CoroutineToken tkn;

        public DelegateNoArg on_Init_Complete;
        public IEnumerator Start_init(string cmd)
        {
            tkn = new CoroutineToken();
            tkn.on_cancel += on_TokenCancelled;
            if (!StoreUserResults(cmd))
            {
                tkn.Cancel();
                yield break; //TODO handle problem with message
            }
            Iinitializer map = SetupMapManager();

            List<Iinitializer> initializers = new List<Iinitializer>()
            {
                map
            };
            yield return map.Process_Init(tkn);
            
            
            InitCleanup(initializers);
        }
        
        private Iinitializer SetupMapManager()
        {
            GameObject Baseobj = new GameObject("Grid");
            Baseobj.AddComponent<Grid>();
            GameObject ChildObj = new GameObject("MapManager");
            ChildObj.transform.SetParent(Baseobj.transform);
            ChildObj.AddComponent<Tilemap>();
            ChildObj.AddComponent<TilemapRenderer>();
            return ChildObj.AddComponent<MapManager>();
        }
        private bool StoreUserResults(string cmd)
        {
            JObject userDetails = JObject.Parse(cmd);
            if (userDetails != null)
            {
                m_userKey = userDetails["userkey"].ToString();
                m_mapID = userDetails["Poid"].ToObject<int>();
                m_region = userDetails["Region"].ToString();
            }

            return userDetails != null;
        }

        private void InitCleanup(IEnumerable<Iinitializer> initializers)
        {
            foreach (Iinitializer initializer in initializers)
            {
                initializer.Init_CleanUp();
            }
        }
        private void on_TokenCancelled()
        {
            tkn.on_cancel -= on_TokenCancelled;
            StopAllCoroutines();
            Debug.LogError("initializer token was cancelled");
        }


    }
}