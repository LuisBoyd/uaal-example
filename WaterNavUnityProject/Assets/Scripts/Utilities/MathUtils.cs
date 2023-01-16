using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static float Normalize(float minValue, float maxValue, float currentValue) =>
            (currentValue - minValue) / (maxValue - minValue);

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

        public static float[] PolynomialMultiplication(float[] termA = null, float[] termB = null)
        {
            if (termA == null || termB == null)
                return null;

            float[] Output = new float[termA.Length + termB.Length - 1];
            for (int i = 0; i < termA.Length + termB.Length - 1; i++)
            {
                Output[i] = 0;
            }
            

            for (int i = 0; i < termA.Length; i++)
            {
                for (int j = 0; j < termB.Length; j++)
                {
                    Output[i + j] += termA[i] * termB[j];
                }
            }

            return Output;
        }
        public static string PrintPoly(float[] poly)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < poly.Length; i++)
            {
                builder.Append(poly[i]);
                if (i != 0)
                    builder.Append($"x^{i}");
                if (i != poly.Length - 1)
                    builder.Append(" + ");
            }

            return builder.ToString();
        }
        
        
        public static int[][] PascalTriangleLookUpTable = new int[][]
        {
            new int[] { 1 }, //n = 0
            new int[] {1,1}, //n = 1
            new int[] {1,2,1}, //n = 2
            new int[] {1,3,3,1}, //n = 3
            new int[] {1,4,6,4,1}, //n = 4
            new int[] {1,5,10,10,5,1}, //n = 5
            new int[] {1,6,15,20,15,6,1}, //n = 6
            new int[] {1,7,21,35,35,21,7,1}, //n = 7
            new int[] {1,8,28,56,70,56,28,8,1}, //n = 8
        };

        public static int BinomialLookup(int n, int k)
        {
            while (n >= PascalTriangleLookUpTable.Length)
            {
                int s = PascalTriangleLookUpTable.Length;
                int[] nextRow = new int[s + 1];
                nextRow[0] = 1;
                for (int i = 1, prev = s-1; i < s; i++)
                {
                    nextRow[i] = PascalTriangleLookUpTable[prev][i - 1] +
                                 PascalTriangleLookUpTable[prev][i];
                }

                nextRow[s] = 1;
                Array.Resize(ref PascalTriangleLookUpTable, s + 1);
                PascalTriangleLookUpTable[s] = nextRow;

            }

            return PascalTriangleLookUpTable[n][k];
        }

        private static float Bernstein(int n, int k, float t)
        {
            float t_k = Mathf.Pow(t, k);
            float t_n_minus_k = Mathf.Pow((1 - t), (n - k));

            float basis = BinomialLookup(n, k) * t_k * t_n_minus_k;
            return basis;
        }

        public static List<Vector3> BezierCurvePointPath(List<Vector3> ControlPoints, float interval = 0.01f)
        {
            int n = ControlPoints.Count - 1;
            List<Vector3> points = new List<Vector3>();
            for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t+= interval)
            {
                Vector3 p = new Vector3();
                for (int i = 0; i < ControlPoints.Count; i++)
                {
                    Vector3 bn = Bernstein(n, i, t) * ControlPoints[i];
                    p += bn;
                }
                points.Add(p);
            }

            return points;
        }
        
        public static List<Vector2> BezierCurvePointPath2D(List<Vector3> controlPoints, float interval = 0.01f)
        {
            int n = controlPoints.Count - 1;
            List<Vector2> points = new List<Vector2>();
            for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t+= interval)
            {
                Vector2 p = new Vector2();
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    Vector2 bn = Bernstein(n, i, t) * controlPoints[i];
                    p += bn;
                }
                points.Add(p);
            }

            return points;
        }
        
        public static List<Vector2> BezierCurvePointPath2D(Transform transform,List<Vector3> controlPoints, float interval = 0.01f)
        {
            int n = controlPoints.Count - 1;
            List<Vector2> points = new List<Vector2>();
            points.Add(transform.position);
            for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t+= interval)
            {
                Vector2 p = new Vector2();
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    Vector2 bn = Bernstein(n, i, t) * controlPoints[i];
                    p += bn;
                }
                points.Add(p);
            }

            return points;
        }
        
        public static List<Vector3> BezierCurvePointPath(Transform transform,List<Vector3> ControlPoints, float interval = 0.01f)
        {
            int n = ControlPoints.Count - 1;
            List<Vector3> points = new List<Vector3>();
            points.Add(transform.position);
            for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t+= interval)
            {
                Vector3 p = new Vector3();
                for (int i = 0; i < ControlPoints.Count; i++)
                {
                    Vector3 bn = Bernstein(n, i, t) * ControlPoints[i];
                    p += bn;
                }
                points.Add(p);
            }

            return points;
        }

        public static Vector3[,] SplitCubicCurve(Vector3[] points)
        {
            if (points.Length < 4 || points.Length > 4)
                return null;
            Vector3[,] result = new Vector3[2, 4];
            Vector3 E, F, G, H, J, K;

            E = (points[0] + points[1]) / 2;
            F = (points[1] + points[2]) / 2;
            G = (points[2] + points[3]) / 2;
            H = (E + F) / 2;
            J = (F + G) / 2;
            K = (H + J) / 2;

            result[0, 0] = points[0];
            result[0, 1] = E;
            result[0, 2] = H;
            result[0, 3] = K;
            result[1, 0] = points[3];
            result[1, 0] = G;
            result[1, 0] = J;
            result[1, 0] = K;

            return result;

        }
        
        // public static List<Vector3> SplitCubicBezierCurve(List<Vector3> controlPoints, int splitCount)
        // {
        //     int e = controlPoints.Count / 4; //e = amount of sets of Curves there are.
        //     if (e >= splitCount)
        //         return controlPoints;
        //
        //     int i = 0;
        //     int depth = 0;
        //     Dictionary<int, Vector3[]> depth_subTree = new Dictionary<int, Vector3[]>();
        //     Vector3[] subPoints = SplitSubPoints(controlPoints);
        //     i = subPoints.Length;
        //     depth_subTree.Add(depth, subPoints);
        //     depth++;
        //     while (i != 1)
        //     {
        //         subPoints = SplitSubPoints(depth_subTree[depth - 1]);
        //         depth_subTree.Add(depth, subPoints);
        //         i = subPoints.Length;
        //         depth++;
        //     }
        //     
        // }
        //
        // private static Vector3[] SplitSubPoints(List<Vector3> controlPoints, int index = 0)
        // {
        //     int q = (controlPoints.Count / 2) - 1; //q = the amount of pairs there are e.g 4 control points A,B,C,D has AB,BC,CD
        //     Vector3[] p = new Vector3[q]; //p are the points in between the pairs based on the value of t
        //     for (int i = index; i < q; i++)
        //         p[i] = (controlPoints[i] + controlPoints[i + 1]) / 2; //Get the basePoint and the next one and work out the midpoint
        //     return p;
        // }
        //
        // private static Vector3[] SplitSubPoints(Vector3[] controlPoints, int index = 0)
        // {
        //     int q = (controlPoints.Length / 2) - 1; //q = the amount of pairs there are e.g 4 control points A,B,C,D has AB,BC,CD
        //     Vector3[] p = new Vector3[q]; //p are the points in between the pairs based on the value of t
        //     for (int i = index; i < q; i++)
        //         p[i] = (controlPoints[i] + controlPoints[i + 1]) / 2; //Get the basePoint and the next one and work out the midpoint
        //     return p;
        // }

        /// <summary>
        /// Linear interpolate between 2 points by t
        /// </summary>
        /// <param name="a">Point A the start of the line</param>
        /// <param name="b">Point B the end of the line</param>
        /// <param name="t">the percentage in-between the two points value between 0 and 1</param>
        /// <returns>the vector that is t * B from A (the vector that lies in between the 2 points)</returns>
        public static Vector3 LinerInterpolation(Vector3 a, Vector3 b, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return oneMinusT * a + t * b;
        }

        /// <summary>
        /// Will Compare 2 floating point values with a margin of 0.001% difference
        /// In which cas it's most likely they are equal
        /// </summary>
        /// <returns></returns>
        public static bool FloatingPointComparision(double a, double b)
        {
            double diffrence = Math.Abs(a * 0.00001);
            if (Math.Abs(a - b) <= diffrence)
                return true;
            return false;
        }
        /// <summary>
        /// Will Compare 2 floating point values with a margin of 0.001% difference
        /// In which cas it's most likely they are equal
        /// </summary>
        /// <returns></returns>
        public static bool FloatingPointComparision(float a, float b)
        {
            float diffrence = MathF.Abs(a * 0.00001f);
            if (MathF.Abs(a - b) <= diffrence)
                return true;

            return false;
        }
        
        
    }
}