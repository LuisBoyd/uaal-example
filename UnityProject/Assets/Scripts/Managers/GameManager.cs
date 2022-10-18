using System.Collections.Generic;
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

        protected override void Awake()
        {
            base.Awake();
            NativeBridge.Instance.InsertCMD("LL", LoadLocation);
            NativeBridge.Instance.InsertCMD("Init", Initialize);
            
        }

        private void Initialize(string cmd)
        {
            JObject InitCMD = JObject.Parse(cmd);
            if (InitCMD != null)
            {
                Region = InitCMD["Region"].ToObject<string>();
            }
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
                LoggingUtils.Instance.Debug(json);
            }
            //TODO after I have the json look at the "Filemangager to find the relevant map file if not create a default one" look at whiteboard at home
        }
        
        
    }
}