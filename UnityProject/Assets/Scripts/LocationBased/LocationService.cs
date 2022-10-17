using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationService : MonoBehaviour
{
    private const int EarthRadius = 6371;

    private const double RCRLat = 52.80143268623349;
    private const double RCRLong = -2.082957707818079;

    private LocationInfo m_StaticLocation;
    
    public Text text;    
    void appendToText(string line) { text.text += line + "\n"; }

    private IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            appendToText("User has not enabled location services");
            yield break;
        }
        
        Input.location.Start();

        int MaxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing
               && MaxWait > 0)
        {
            yield return new WaitForSeconds(1);
            MaxWait--;
        }

        if (MaxWait < 1)
        {
            appendToText("Location Services Timed Out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            appendToText("Unable to determine Device Location");
            yield break;
        }
        else
        {
            StartCoroutine(StartTracking());
        }

    }

    private IEnumerator StartTracking()
    {
        while (Haversine(Input.location.lastData.latitude, Input.location.lastData.longitude,
                   (float)RCRLat,(float)RCRLong) < 1.56f)
        {
            appendToText($"Lat:{Input.location.lastData.latitude} Long:{Input.location.lastData.longitude}");
            yield return new WaitForSeconds(1);
        }
        Input.location.Stop();
    }

    private float Haversine(LocationInfo a, LocationInfo b)
    {
        float dLat = (b.latitude - a.latitude) * Mathf.Deg2Rad;
        float dLon = (b.longitude - a.longitude) * Mathf.Deg2Rad;

        var c = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2)
                + Mathf.Cos(a.latitude * Mathf.Deg2Rad) * Mathf.Cos(b.latitude
                                                                    * Mathf.Deg2Rad) * Mathf.Sin(dLon / 2) *
                Mathf.Sin(dLon / 2);

        var e = 2 * Mathf.Atan2(Mathf.Sqrt(c), Mathf.Sqrt(1 - c));
        var d = EarthRadius * e;
        return d;
    }
    
    private float Haversine(float ALat, float ALong, float BLat, float BLong)
    {
        float dLat = (BLat - ALat) * Mathf.Deg2Rad;
        float dLon = (BLong - ALong) * Mathf.Deg2Rad;

        var c = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2)
                + Mathf.Cos(ALat * Mathf.Deg2Rad) * Mathf.Cos(BLat
                                                                    * Mathf.Deg2Rad) * Mathf.Sin(dLon / 2) *
                Mathf.Sin(dLon / 2);

        var e = 2 * Mathf.Atan2(Mathf.Sqrt(c), Mathf.Sqrt(1 - c));
        var d = EarthRadius * e;
        return d;
    }
}
