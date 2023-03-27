using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core.Shopping;
using UnityEngine;

namespace RCRCoreLib.Core.Utilities
{
    public class GraphHelper
    {
        /// <summary>
        /// Gets the Minimum Y point on a 2d graph
        /// </summary>
        /// <param name="points">Points plotted on the graph</param>
        /// <returns>The point with the lowest Y value (if multiple exist then with the lowest X value as well)</returns>
        public static Vector3 GetMinY(IEnumerable<Vector3> points)
        {
            var point = from vec in points
                orderby vec.y, vec.x
                select vec;
            
            //Order the Collection By Y Then X and after wards return the first element in that collection.

            return point.First();
        }

        public static IEnumerable<Vector3> SortByAngleMin(IEnumerable<Vector3> points, Vector3 pointToCompare)
        {
            var orderedSet = points.OrderBy((v) => MathHelper.Angle(pointToCompare, v)); //2d sort.
            return orderedSet;
        }

        /// <summary>
        /// https://demonstrations.wolfram.com/GrahamScanToFindTheConvexHullOfASetOfPointsIn2D/ scan done here
        /// </summary>
        /// <param name="points"></param>
        /// <returns>the convex hull of the collection</returns>
        public static IEnumerable<Vector3> GrahamScan(IEnumerable<Vector3> points)
        {
            //InternalPoints
            var internalPoints = points.ToList();
            //Get MinimumY Points from points collection
            Vector3 startingPoint = GetMinY(internalPoints);
            //Remove Starting point from Internal list
            internalPoints.Remove(startingPoint);
            //Get sorted List based on X polar angle from starting point
            internalPoints = SortByAngleMin(internalPoints, startingPoint).ToList();

            //Create Stack and add the starting point and the first of the sorted points
            Stack<Vector3> convexHull = new Stack<Vector3>();
            convexHull.Push(startingPoint);
            convexHull.Push(internalPoints[0]);
            internalPoints.RemoveAt(0); //Remove what was just pushed onto the stack from the internal list

            for (int p = 0; p < internalPoints.Count; p++)
            {
                while (convexHull.Count >= 2 &&
                       GrahamScan_orientation(convexHull.ElementAt(0), convexHull.ElementAt(1), internalPoints[p]) != 1)
                    convexHull.Pop();
                convexHull.Push(internalPoints[p]);
            }
            return convexHull;
        }

        private static int GrahamScan_orientation(Vector3 a, Vector3 b, Vector3 c)
        {
            float dir = (c.y - b.y) * (b.x - a.x) - (b.y - a.y) * (c.x - b.x);
            if (dir > 0)
                return 1;
            if (dir < 0)
                return -1;

            return 0;
        }
    }
}