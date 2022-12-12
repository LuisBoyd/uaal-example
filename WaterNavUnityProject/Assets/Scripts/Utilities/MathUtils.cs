using System;
using UnityEngine;

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


        /// <summary>
        /// Determines if the floating point Value is whole and could easily be cast to Type Int
        /// </summary>
        /// <param name="num">Floating point value</param>
        /// <returns>True if number is whole</returns>
        public static bool IsFloatWhole(float num)
        {
            return (num % 1) == 0;
        }

        /// <summary>
        /// Returns the Count of Division
        /// </summary>
        public static int DivisionInto(int total, int divisionnumber)
        {
            return total / divisionnumber;
        }

        public static int sqrt(int value)
        {
            return Mathf.FloorToInt(Mathf.Sqrt(value));
        }

        public static int sqrt(float value)
        {
            return Mathf.FloorToInt(Mathf.Sqrt(value));
        }

        public static Tuple<int,int> GetMedian(float number)
        {
            if (IsFloatWhole(number))
            {
                if (!IsFloatWhole((number + 1f) / 2f))
                {
                    int lower = Mathf.FloorToInt((number + 1f) / 2f);
                    int Higher = Mathf.CeilToInt((number + 1f) / 2f);
                    return Tuple.Create(lower, Higher);
                }
                else
                {
                    return Tuple.Create(Mathf.FloorToInt((number + 1f) / 2f), 0);
                }
            }
            return Tuple.Create(0,0);
        }

        /// <summary>
        /// If one value is a multiple of the other
        /// </summary>
        /// <param name="a">first number if this is a multiple of b</param>
        /// <param name="b">second number if a is a multiple of this number</param>
        /// <returns>is "a" multiple of "b"</returns>
        public static bool IsXMultipleOfY(int a, int b)
        {
            return a % b == 0;
        }
    }
}