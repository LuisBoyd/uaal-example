using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DataStructures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RCR.BaseClasses;
using RCR.Utilities;
using UnityEngine;

namespace RCR.Managers
{
    /// <summary>
    /// Main "Program Class" First Execution Happens here
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class GameManager: Singelton<GameManager>
    {
        public string Region { get; private set; }
        public string Email { get; private set; }

        private Player m_player;
        
        private struct InitDataDummy
        {
            public string Email;
            public string Region;
        }
        
        private struct DummyLocationData
        {
            public string POIid;
        }

        protected override void Awake()
        {
            base.Awake();
            NativeBridge.Instance.InsertCMD("LL", LoadLocation);
            NativeBridge.Instance.InsertCMD("Init", Initialize);
            InitDataDummy dummydata = new InitDataDummy();
            DummyLocationData dummyLocationData = new DummyLocationData();
            dummydata.Email = "luis@rivercanalrescue.co.uk"; //Dummy Info
            dummydata.Region = "UK_EnglandWales";
            dummyLocationData.POIid = "137316";
            Initialize(JsonConvert.SerializeObject(dummydata));
            LoadLocation(JsonConvert.SerializeObject(dummyLocationData));
            

        }

        private void Initialize(string cmd)
        {
            //TODO if INIT CMD fails close game
            JObject InitCMD = JObject.Parse(cmd);
            if (InitCMD != null)
            {
                Region = InitCMD["Region"].ToString();
                Email = InitCMD["Email"].ToString();//Dummy Info
                    //InitCMD["Email"].ToString();

                StartCoroutine(NetworkManager.Instance.PutRequest("CheckUser",
                    new Dictionary<string, string>()
                    {
                        { "UserKey", Email }
                    },
                    onUserCheck_CallBack));
            }
        }

        private void onUserCheck_CallBack(bool success, string json)
        {
            if (!success)
            {
                LoggingUtils.Instance.Debug("Failed To Check User");
                return;
            }
            
            
            JObject response = JObject.Parse(json);
            if (response != null)
            {
                bool UserExists = response["Status"].ToObject<bool>();
                if (UserExists)
                {
                    //TODO Create Player Object with populated player data
                    StartCoroutine(NetworkManager.Instance
                        .PutRequest("GetPlayerData",
                            new Dictionary<string, string>()
                            {
                                { "UserKey", Email }
                            },
                            on_GetPlayerData));
                }
                else
                {
                    //TODO Prompt a player to Create a user Instead
                    //TODO Create new user then populate with player data (Done)
                    StartCoroutine(NetworkManager.Instance.PutRequest(
                        "CreateUser",
                        new Dictionary<string, string>()
                        {
                            { "UserKey", Email }
                        }, on_CreatedUserCallBack));
                }
            }
        }

        private void on_CreatedUserCallBack(bool success, string json)
        {
            if (!success)
            {
                LoggingUtils.Instance.Debug("Failed To Create New User");
                return;
            }

            JObject response = JObject.Parse(json);
            if (response != null)
            {
                bool Created = response["Status"].ToObject<bool>();

                if (Created)
                {
                    //TODO Get newly created Data and put it in player
                    StartCoroutine(NetworkManager.Instance
                        .PutRequest("GetPlayerData",
                            new Dictionary<string, string>()
                            {
                                { "UserKey", Email }
                            },
                            on_GetPlayerData));
                }
                else
                {
                    //TODO Cutoff Game??
                    Application.Quit();
                }
            }
            
        }

        private void on_GetPlayerData(bool success, string json)
        {
            if (!success)
            {
                LoggingUtils.Instance.Debug("Failed To Get user Details");
                return;
            }
            m_player = new Player(json);
            LoggingUtils.Instance.Debug($"Email : {m_player.m_email}");
            LoggingUtils.Instance.Debug($"Last login : {m_player.m_lastlogin.ToString(DateTimeFormatInfo.CurrentInfo)}");
            LoggingUtils.Instance.Debug($"Current Money : {m_player.m_currentCurrency.ToString()}");
            LoggingUtils.Instance.Debug($"Premium Currency : {m_player.m_currentPremiumCurrency}");

            
        }
        
        private void LoadLocation(string CMD)
        {
            if (!string.IsNullOrEmpty(Region))
            {
                JObject LocationCMD = JObject.Parse(CMD);
                if (LocationCMD != null)
                {
                    StartCoroutine(NetworkManager.Instance.PutRequest("RequestPOIData",
                        new Dictionary<string, string>()
                        {
                            { "database", Region },
                            { "POIid", LocationCMD["POIid"].ToString() }
                        }, OnLoadLoaction_Callback));
                }
            }
        }

        private void OnLoadLoaction_Callback(bool success, string json)
        {
            if (!success)
            {
                LoggingUtils.Instance.Debug("Failed Callback");
                return;
            }
            else
            {
                JObject response = JObject.Parse(json);
                if (response != null)
                {
                    StartCoroutine(NetworkManager.Instance.PutRequest("RequestGameLocationData",
                        new Dictionary<string, string>()
                        {
                            { "Region", Region },
                            { "POIid", response["POIid"].ToString() },
                            { "userkey", Email }
                        }, On_RecivedGameLocationData));
                    //LoggingUtils.Instance.Debug(json);
                }
            }
            //TODO after I have the json look at the "Filemangager to find the relevant map file if not create a default one" look at whiteboard at home
        }

        private void On_RecivedGameLocationData(bool success, string json)
        {
            if (!success)
            {
                LoggingUtils.Instance.Debug("Failed Callback");
                return;
            }
            else
            {
                JObject response = JObject.Parse(json);
                if (response != null)
                {
                    LoggingUtils.Instance.Debug($"id : {response["id"].ToString()}");
                    LoggingUtils.Instance.Debug($"owner : {response["owner"].ToString()}");
                    LoggingUtils.Instance.Debug($"mapid: {response["mapid"].ToString()}");
                    LoggingUtils.Instance.Debug($"area : {response["area"].ToString()}");
                    LoggingUtils.Instance.Debug($"name : {response["name"].ToString()}");
                    
                    //TODO check if 1. we have all the area files, 2.check if we have the mapfile for this area
                    //TODO change area to pherhaps json in the sql server and check to see if that works
                }
            }
        }
        
        
    }
}