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
#if UNITY_EDITOR

    /// <summary>
    /// TODO remove for Builds
    /// </summary>
    private void Start()
    {
        JObject DummyData = new JObject();
        DummyData.Add("userkey", "luis@rivercanalrescue.co.uk");
        DummyData.Add("Poid", 137243);
        DummyData.Add("Region", "UK_EnglandWales");
        
        LoadLocation(DummyData.ToString());
    }
#endif

    void LoadLocation(string cmd) => GameManager.Instance.StartCoroutine(
        GameManager.Instance.Start_init(cmd));

    void LoadGameData(string cmd)
    {
        //TODO UserMapData
        //TODO BuildingData just enough to construct all building objects so maybe need a factory for that
        //TODO Load User Details as well just like money and such anything related to the user.
        
    }

    /// <summary>
    /// Callback Method passed to the network Manager
    /// </summary>
    // private void onLoadLocation_Complete(bool value, string response)
    // {
    //     if (!value)
    //         ExitApplication();
    //     
    //     JObject LocationResponseObj = JObject.Parse(response);
    //     if (LocationResponseObj != null)
    //     {
    //         string AreaData = LocationResponseObj["MapData"].ToString();
    //         Debug.Log($"THIS IS AREA DATA {AreaData}");
    //         int[] int_Area_data = AreaData.Split(',').Select(int.Parse).ToArray();
    //         MapManager.Instance.LoadAreaData(int_Area_data);
    //     }
    //     else
    //     {
    //         ExitApplication();
    //     }
    // }

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
