using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using RCR.BaseClasses;
using RCR.Managers;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-2)]
public class NativeBridge : Singelton<NativeBridge>
{

    private Dictionary<string, Action<string>> m_proc;

    private const int MAX_AREA_COUNT = 25;

    protected override void Awake()
    {
        base.Awake();
        m_proc = new Dictionary<string, Action<string>>();
    }

    // void appendToText(string line) { text.text += line + "\n"; }

    public void InsertCMD(string CMDIdentifier, Action<string> callback)
    {
        if(!m_proc.ContainsKey(CMDIdentifier))
            m_proc.Add(CMDIdentifier, callback);
        
        //Else already in there
    }

    public void RemoveCMD(string CMDIdentifier)
    {
        if (m_proc.ContainsKey(CMDIdentifier))
            m_proc.Remove(CMDIdentifier);
        
        //Else Could not Remove From Dictionary
    }
    
    // void AppendLocation(string cmd)
    // {
    //     appendToText($"Changing Location to {cmd}");
    // }

    void RecieveCMD(string cmd)
    {
        foreach (var keyValue in m_proc)
        {
            if (cmd.StartsWith(keyValue.Key))
            {
                string[] splitcmd = cmd.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < splitcmd.Length; i++)
                {
                    sb.Append($"{splitcmd[i]} ");
                }

                m_proc[splitcmd[0]](sb.ToString());

            }
        }
    }

    void RecieveJSONCMD(string cmd)
    {
        Debug.Log(cmd);
        JObject JsonCMD = JObject.Parse(cmd);
        if (JsonCMD != null)
        {
            string CMDCode = JsonCMD["Code"].ToString();
            if (!string.IsNullOrEmpty(CMDCode))
            {
                if (m_proc.ContainsKey(CMDCode))
                    m_proc[CMDCode](JsonCMD.ToString());
            }
        }
    }

    private event OnOutOfPOIRange m_rangeLeft;
    public event OnOutOfPOIRange RangeLeft
    {
        add
        {
            Debug.Log($"{value.Method.Name} Subscribed To RangeLeft");
            m_rangeLeft += value;
        }
        remove
        {
            Debug.Log($"{value.Method.Name} Un-Subscribed To RangeLeft");
            m_rangeLeft -= value;
        }
    }
    
    /// <summary>
    /// To be called from The Native Side in A background process to indicate we have left the range
    /// of a POI
    /// </summary>
    /// <param name="cmd"></param>
    void OnExitRange(string cmd)
    {
        m_rangeLeft?.Invoke();
    }

    void LoadLocation(string cmd)
    {
        JObject passOverObj = JObject.Parse(cmd);
        if (passOverObj != null)
        {
            GameManager.Instance.PoulateData(passOverObj["userkey"].ToString(),  passOverObj["Poid"].ToString(), passOverObj["Region"].ToString());
            StartCoroutine(NetworkManager.Instance.PutRequest("RequestUserMapData",
                new Dictionary<string, string>
                {
                    { "userKey", passOverObj["userkey"].ToString() },
                    { "POIID", passOverObj["Poid"].ToString() },
                    {"region", passOverObj["Region"].ToString()}
                }, onLoadLocation_Complete));
        }
        else
        {
            //TODO EXIT out of game no need to launch or save anything
            ExitApplication();
        }
    }

    /// <summary>
    /// Callback Method passed to the network Manager
    /// </summary>
    private void onLoadLocation_Complete(bool value, string response)
    {
        if (!value)
            ExitApplication();
        
        JObject LocationResponseObj = JObject.Parse(response);
        if (LocationResponseObj != null)
        {
            string AreaData = LocationResponseObj["MapData"].ToString();
            Debug.Log($"THIS IS AREA DATA {AreaData}");
            int[] int_Area_data = AreaData.Split(',').Select(int.Parse).ToArray();
            GameManager.Instance.ProcessAreaData(int_Area_data);
        }
        else
        {
            ExitApplication();
        }
    }

    private void ExitApplication()
    {
        Application.Quit();
    }


    /// <summary>
    /// Verify's the RCR Members access to the passed in Location
    /// </summary>
    /// <param name="cmd">A JSON Value containing the User Key and the Location Key to enter</param>
    void VerifyEntry(string cmd)
    {
        //TODO Implement Method
        //Should mostly be done on the Native side before entry to the app
    }
    
    
    #region CommandsList

    /*
     *
     * Init ["Init JSON"]
     * "LL [LocationID]" LL is code for Load Location
     * "Debug ["Message"]" Debug is code for printing out something in game
     * 
     */

    #endregion
}
