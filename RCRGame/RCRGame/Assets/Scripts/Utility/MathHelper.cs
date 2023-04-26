using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public static class MathHelper
    {
        /// <summary>
        ///  converts a integer value to a string that represents it to x amount of decimal places
        /// </summary>
        public static string ToDecimalPlace(int value, int decimalPlacePoint) =>
            (value / (decimal) Math.Pow(10.00, decimalPlacePoint)).ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///  converts a integer value into a decimal category so for example 10 would return tenths
        /// *Supports a standard int32 so the max value would be 2147483647 which is billions*
        /// </summary>
        public static string DeciamlCatergory(int value)
        {
            switch (value)
            {
                case < 1000:
                    return string.Empty;
                    break;
                case < 1000000:
                    return "Thousands";
                    break;
                case < 1000000000:
                    return "Millions";
                    break;
                // case < 1000000000:
                //     return "Trillions"; not supported by int32 unfortunately
                //     break;
                case 2147483647:
                    return "Maxed";
                break;
            }
            return string.Empty;
        }
        
        /// <summary>
        ///  converts a integer value into a decimal category shorthand so for example 1000 would return K or 1 million would return M
        /// *Supports a standard int32 so the max value would be 2147483647 which is billions*
        /// </summary>
        public static string DeciamlCatergoryShortHand(int value)
        {
            switch (value)
            {
                case < 1000:
                    return string.Empty;
                    break;
                case < 1000000:
                    return "K";
                    break;
                case < 1000000000:
                    return "M";
                    break;
                // case < 1000000000:
                //     return "Trillions"; not supported by int32 unfortunately
                //     break;
                case 2147483647:
                    return "Max";
                    break;
            }
            return string.Empty;
        }

        private const int ConvexInf = 10000;

        /// <summary>
        /// to find the orientation of the ordered vector 2.
        /// The function returns following values 0 --> p, q and r are collinear  1 --> Clockwise
        /// </summary>
        static int Orentation(Vector2 p, Vector2 q, Vector2 r)
        {
            float val = (r.y - q.y) * (q.x - p.x) - (q.y - p.y) * (r.x - q.x);

            if (val > 0)
                return 1; //counterClockwise
            else if (val < 0)
                return -1; // clockwise
            else
                return 0; //Collinear
        }

        public static List<Vector3> ConvexHull_GrahamScan(Vector3[] points, int n)
        {
            if (n < 3) return null; //there must be at least 3 points
            List<Vector3> hull = new List<Vector3>();

            Vector3 p0 = points.OrderBy(point => point.y).ThenBy(point => point.x).First();
            var orderedPoints = points.OrderBy(p => p.PolarAngle(p0)).ToList();

            for (int i = 0; i < orderedPoints.Count; i++)
            {
                while (hull.Count >= 2 && Orentation(hull[^2], hull[^1], orderedPoints[i]) != 1)
                {
                    hull.RemoveAt(hull.Count - 1);
                }
                hull.Add(orderedPoints[i]);
            }
            return hull;
        }
        public static List<Vector2> ConvexHull_GrahamScan2D(Vector3[] points)
        {
            if (points.Length < 3) return null; //there must be at least 3 points
            List<Vector2> hull = new List<Vector2>();

            Vector3 p0 = points.OrderBy(point => point.y).ThenBy(point => point.x).First();
            var orderedPoints = points.OrderBy(p => p.PolarAngle(p0)).ToList();

            for (int i = 0; i < orderedPoints.Count; i++)
            {
                while (hull.Count >= 2 && Orentation(hull[^2], hull[^1], orderedPoints[i]) != 1)
                {
                    hull.RemoveAt(hull.Count - 1);
                }
                hull.Add(orderedPoints[i]);
            }
            return hull;
        }

        public static double PolarAngle(this Vector3 point, Vector3 refrencePoint)
        {
            double dx = point.x - refrencePoint.x;
            double dy = point.y - refrencePoint.y;
            return Math.Atan2(dy, dx);
        }
        
        public static double PolarAngle(this Vector2 point, Vector2 refrencePoint)
        {
            double dx = point.x - refrencePoint.x;
            double dy = point.y - refrencePoint.y;
            return Math.Atan2(dy, dx);
        }
        
        public static IEnumerable<Vector3> SortByPolarAngle(this IEnumerable<Vector3> points, Vector3 refrencePoint)
        {
            return points.OrderBy(p => p.PolarAngle(refrencePoint));
        }
        
        public static IEnumerable<Vector2> SortByPolarAngle(this IEnumerable<Vector2> points, Vector2 refrencePoint)
        {
            return points.OrderBy(p => p.PolarAngle(refrencePoint));
        }

        /// <summary>
        /// Tightly Wraps all the points by removing all duplicate point values from a list.
        /// </summary>
        public static IEnumerable<Vector3> RemoveDuplicatePoints(this IEnumerable<Vector3> collection)
        {
            var enumerable = collection.ToList();
            var duplicatePoints = enumerable.GroupBy(p => new Vector3(p.x, p.y, p.z))
                .Where(g => g.Count() > 1)
                .SelectMany(g => g);
            return enumerable.Except(duplicatePoints).ToList();
        }
        public static IEnumerable<Vector2> RemoveDuplicatePoints(this IEnumerable<Vector2> collection)
        {
            var enumerable = collection.ToList();
            var duplicatePoints = enumerable.GroupBy(p => new Vector3(p.x, p.y))
                .Where(g => g.Count() > 1)
                .SelectMany(g => g);
            return enumerable.Except(duplicatePoints).ToList();
        }
        
        
        /// <summary>
        /// Removes entry from collection if it shows up more than X times.
        /// </summary>
        public static IEnumerable<Vector2> RemoveEntryWithXDuplicates(this IEnumerable<Vector2> collection, int x)
        {
            var enumerable = collection.ToList();
            var duplicatePoints = enumerable.GroupBy(p => new Vector3(p.x, p.y))
                .Where(g => g.Count() > x)
                .SelectMany(g => g);
            return enumerable.Except(duplicatePoints).ToList();
        }

    }
}