using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using RCR.BaseClasses;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-2)]
public class NativeBridge : Singelton<NativeBridge>
{

    private Dictionary<string, Action<string>> m_proc;

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
