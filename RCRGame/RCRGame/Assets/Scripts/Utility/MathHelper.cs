using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Clipper2Lib;
using Core.models.maths;
using Pathfinding.ClipperLib;
using UnityEngine;
using Clipper = Clipper2Lib.Clipper;
using ClipType = Clipper2Lib.ClipType;
using Random = UnityEngine.Random;

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
        static int Orientation(Vector2 p, Vector2 q, Vector2 r)
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
                while (hull.Count >= 2 && Orientation(hull[^2], hull[^1], orderedPoints[i]) != 1)
                {
                    hull.RemoveAt(hull.Count - 1);
                }
                hull.Add(orderedPoints[i]);
            }
            return hull;
        }
        public static List<Vector2> ConvexHull_GrahamScan2D(Vector2[] points)
        {
            if (points.Length < 3) return null; //there must be at least 3 points
            List<Vector2> hull = new List<Vector2>();

            Vector3 p0 = points.OrderBy(point => point.y).ThenBy(point => point.x).First();
            var orderedPoints = points.OrderBy(p => p.PolarAngle(p0)).ToList();

            for (int i = 0; i < orderedPoints.Count; i++)
            {
                while (hull.Count >= 2 && Orientation(hull[^2], hull[^1], orderedPoints[i]) != 1)
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

        public static IEnumerable<Line> RemoveDuplicateLines(this IEnumerable<Line> lines)
        {
            var enumerable = lines.ToList();
            var duplicatePoints = enumerable.GroupBy(p => p.GetHashCode())
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

        public static Vector2 GeneratePointInsidePolygon(this PolygonCollider2D polygon)
        {
            Vector2 minVec = MinPointOnThePolygon(polygon.points);
            Vector2 maxVec = MaxPointOnThePolygon(polygon.points);
            Vector2 GenVector;

            float x = ((Random.value) * (maxVec.x - minVec.x)) + minVec.x;
            float y = ((Random.value) * (maxVec.y - minVec.y)) + minVec.y;
            GenVector = new Vector2(x, y);
            while (!polygon.PointInside(GenVector))
            {
                 x = ((Random.value) * (maxVec.x - minVec.x)) + minVec.x;
                 y = ((Random.value) * (maxVec.y - minVec.y)) + minVec.y;
                 GenVector.x = x;
                 GenVector.y = y;
            }

            return GenVector;
        }

        private static Vector2 MaxPointOnThePolygon(Vector2[] polygonPoints)
        {
            float maxX = polygonPoints[0].x;
            float maxY = polygonPoints[0].y;
            for (int i = 1; i < polygonPoints.Length; i++)
            {
                if (maxX < polygonPoints[i].x)
                    maxX = polygonPoints[i].x;
                if (maxY < polygonPoints[i].y)
                    maxY = polygonPoints[i].y;
            }
            return new Vector2(maxX, maxY);
        }
        private static Vector2 MinPointOnThePolygon(Vector2[] polygonPoints)
        {
            float minX = polygonPoints[0].x;
            float minY = polygonPoints[0].y;
            for (int i = 1; i < polygonPoints.Length; i++)
            {
                if (minX > polygonPoints[i].x)
                    minX = polygonPoints[i].x;
                if (minY > polygonPoints[i].y)
                    minY = polygonPoints[i].y;
            }
            return new Vector2(minX, minY);
        }

        public static bool PointInside(this PolygonCollider2D polygon, Vector3 position)
        {
            return PointInside(polygon, new Vector2(position.x, position.y));
        }

        private const int LineRange = 2000;
        public static bool PointInside(this PolygonCollider2D polygon, Vector2 position)
        {
            int n = polygon.points.Length;
            if (n < 3)
                return false;

            Vector2 infPoint = new Vector2(LineRange, position.y); //mathf.infinty
            Line exLine = new Line(position, infPoint);
            int count = 0;
            int i = 0;
            do
            {
                //Forming a Line from two consecutive Points of the polygon.
                Line side = new Line(polygon.points[i], polygon.points[(i + 1) % n]);
                // Color tempColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                // Debug.DrawLine(side._startPoint, exLine._endPoint, tempColor, 5f);
                // Debug.DrawLine(exLine._startPoint, exLine._endPoint, tempColor, 5f);
                if (side.IsIntersection(exLine))
                {
                    if (Orientation(side._startPoint, position, side._endPoint) == 0)
                        return OnLine(side, position);
                    count++;
                }
                i = (i + 1) % n;
            } while (i != 0);

            return count % 2 != 0; //Must be odd.
        }

        public static bool IsIntersection(this Line lineA, Line lineB)
        {
            int orientation1 = Orientation(lineA._startPoint, lineA._endPoint, lineB._startPoint);
            int orientation2 = Orientation(lineA._startPoint, lineA._endPoint, lineB._endPoint);
            int orientation3 = Orientation(lineB._startPoint, lineB._endPoint, lineA._startPoint);
            int orientation4 = Orientation(lineB._startPoint, lineB._endPoint, lineA._endPoint);

            // When intersecting
            if (orientation1 != orientation2 && orientation3 != orientation4)
                return true;

            if (orientation1 == 0 && OnLine(lineA, lineB._startPoint))
                return true;
            if (orientation2 == 0 && OnLine(lineA, lineB._endPoint))
                return true;
            if (orientation3 == 0 && OnLine(lineB, lineA._startPoint))
                return true;
            if (orientation4 == 0 && OnLine(lineB, lineA._endPoint))
                return true;

            return false;
        }

        static bool OnLine(Line line1, Vector2 point)
        {
            if (point.x <= Math.Max(line1._startPoint.x, line1._endPoint.x)
                && point.x <= Math.Min(line1._startPoint.x, line1._endPoint.x)
                && (point.y <= Math.Max(line1._startPoint.y, line1._endPoint.y)
                    && point.y <= Math.Min(line1._startPoint.y, line1._endPoint.y)))
                return true;
            return false;
        }

        public static void SubtractPolygon(this PolygonCollider2D polygon, PolygonCollider2D subtractionPolygon)
        {
            var clipperd = new ClipperD(6);
            var subject = new PathsD();
            var clip = new PathsD();
            
            subject.Add(new PathD(polygon.points.ToList().ConvertAll(point => new PointD(point.x, point.y))));
            clip.Add(new PathD(subtractionPolygon.points.ToList().ConvertAll(point => new PointD(point.x, point.y))));
            
            clipperd.AddSubject(clip);
            clipperd.AddClip(subject);
            var solution = new PathsD();
            clipperd.Execute(ClipType.Xor, FillRule.NonZero, solution);
            
            Debug.Log($"subject {subject.Count}");
            
            polygon.points = solution.First().ConvertAll(point => new Vector2((float) point.x, (float) point.y))
                .ToArray();
        }
        

    }
}