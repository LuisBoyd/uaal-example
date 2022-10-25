﻿using UnityEngine;

namespace RCR.Utilities
{
    public static class MathUtils
    {
        public const int EarthRadius = 6371;
        
        
        public static float Haversine(float ALat, float ALong, float BLat, float BLong)
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
}