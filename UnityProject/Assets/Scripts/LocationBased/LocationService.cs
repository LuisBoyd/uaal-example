using System;
using System.Collections;
using System.Collections.Generic;
using RCR.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace RCR.location
{

    public class LocationService : MonoBehaviour
    {

        private const double RCRLat = 52.80143268623349;
        private const double RCRLong = -2.082957707818079;

        private LocationInfo m_StaticLocation;

        public Text text;

        void appendToText(string line)
        {
            text.text += line + "\n";
        }

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
            while (MathUtils.Haversine(Input.location.lastData.latitude, Input.location.lastData.longitude,
                       (float)RCRLat, (float)RCRLong) < 1.56f)
            {
                appendToText($"Lat:{Input.location.lastData.latitude} Long:{Input.location.lastData.longitude}");
                yield return new WaitForSeconds(1);
            }

            Input.location.Stop();
        }
        
    }
}
