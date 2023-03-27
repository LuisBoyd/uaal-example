using System;
using UnityEngine;

namespace RCRCoreLib.Core.Utilities
{
    public static class MathHelper
    {
        
        /*
         * Calculas INFO
         */

        /// <summary>
        /// Calculates the slope of a line (m = (b.y - a.y) / (b.x - a.x))
        /// </summary>
        /// <returns>the slope of the line</returns>
        public static float SlopeOfLine(Vector2 a, Vector2 b)
        {
            return (b.y - a.y) / (b.x - a.x);
        }
        
        /*
         * Trigonometry INFO
         *  The angle is 60 degrees. We are given the hypotenuse and need to find the adjacent side. This formula which connects these three is:
            cos(angle) = adjacent / hypotenuse
            therefore, cos60 = x / 13
            therefore, x = 13 × cos60 = 6.5
            therefore the length of side x is 6.5cm.
         */

        /// <summary>
        /// Calculates the Angle between 2 vectors
        /// </summary>
        /// <returns>Returns the Angle in degrees</returns>
        public static float Angle(Vector2 a, Vector2 b)
        {
            Debug.DrawLine(a,b, Color.red, 0.5f);
            //Work Out the dot product of a · b first.
            float dotProduct = Dot(a, b);
            
            //Get the Magnitude of Vector a
            float aMagnitude = VecMagnitude(a);
            //Get the Magnitude of Vector B
            float bMagnitude = VecMagnitude(b);
            
            //Calculate the angle theta
            float theta = (float)Math.Acos(dotProduct / (aMagnitude * bMagnitude)); // Calculated as Degrees not Radians

            return theta;
        }

        public static float VecMagnitude(Vector2 v)
        {
            return (float) Math.Sqrt(Math.Pow(v.x, 2) + Math.Pow(v.y, 2));
        }
        
        
        /*
         * DOT PRODUCT INFO
         * a · b =  |a| × |b| × cos(θ)
         *  Where:
            |a| is the magnitude (length) of vector a
            |b| is the magnitude (length) of vector b
            θ is the angle between a and b
         */
        
        /// <summary>
        /// Returns a 3D Dot Product to compare to vectors.
        /// </summary>
        public static float Dot(Vector3 a, Vector3 b)
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }
        /// <summary>
        /// Returns a 2D Dot Product to compare to vectors.
        /// </summary>
        public static float Dot(Vector2 a, Vector2 b)
        {
            return (a.x * b.x) + (a.y * b.y);
        }
        
        
        /*
         * CROSS PRODUCT INFO
         *  a × b = |a| |b| sin(θ) n (Cross product)
         * |a| is the magnitude (length) of vector a
            |b| is the magnitude (length) of vector b
            θ is the angle between a and b
            n is the unit vector at right angles to both a and b
            
            The Maagnitude of the cross product equals the area of a parallelogram
            
            Four primary uses of the cross product are to: 1) calculate the angle ( ) between two vectors, 2) determine a vector normal to a plane, 3) calculate the moment of a force about a point, and 4) calculate the moment of a force about a line.
         */

        /// <summary>
        /// Returns the Cross Product which is another vector that is at a right angle to both inputted vectors
        /// </summary>
        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            Vector3 cross = Vector3.zero;
            cross.x = (a.y * b.z) - (a.z * b.y);
            cross.y = (a.z * b.x) - (a.x * b.z);
            cross.z = (a.x * b.y) - (a.y * b.x);
            return cross;
        }
    }
}