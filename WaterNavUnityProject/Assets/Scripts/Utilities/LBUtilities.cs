using System.Threading.Tasks;

namespace RCR.Utilities
{
    /*
  *  LBUtilities
  *  Usefully Helper Functions for applications I regularly build
  *  if you use without my permission please give credit
  *  Valid for Unity 2019.4.38f
  *
  *  Created by Luis Boyd on 12/01/2023
  */

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class LBUtilities
    {
        /// <summary>
        /// Check to see if passed in reference is null
        /// </summary>
        /// <returns>
        /// True - if not Null, False - if Null
        /// </returns>
        public static bool AssertNull(object obj) => obj != null;

        /// <summary>
        /// Expensive method to do a deep copy of a Unity Component
        /// </summary>
        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }

            return copy as T;
        }

        /// <summary>
        /// Try and cast a class to another class
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="castedValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Cast<T>(object obj, out T castedValue) where T : class
        {
            castedValue = obj as T;
            if (AssertNull(castedValue))
                return true;

            castedValue = null;
            return false;
        }

        public static T SortLargestValue<T>(IEnumerable<T> list) where  T : IComparable<T>
        {
            T largest = default;
            foreach (T obj in list)
            {
                int value = obj.CompareTo(largest);
                if (value > 0)
                    largest = obj;
            }

            return largest;
        }
        public static T SortSmallestValue<T>(IEnumerable<T> list) where  T : IComparable<T>
        {
            T smallest = default;
            foreach (T obj in list)
            {
                int value = obj.CompareTo(smallest);
                if (value < 0)
                    smallest = obj;
            }

            return smallest;
        }

        public static IEnumerator WaitForAsyncTask(Task task)
        {
            yield return new WaitUntil((() => task.IsCompleted));
        }

        public static IEnumerator WaitForAllAsycTasks(IEnumerable<Task> tasks)
        {
            foreach (var task in tasks)
            {
                yield return new WaitUntil((() => task.IsCompleted));
            }
        }

        public static Sprite SpriteFromTexture(Texture2D texture)
        {
            if (!IsPower(texture.width, 2.0f) || !IsPower(texture.height, 2.0f))
            {
                Debug.LogWarning($"{texture.name} is not power of 2");
                return null;
            }
            
            
            return Sprite.Create(texture, new Rect(Vector2.zero,
                    new Vector2(texture.width, texture.height)),
                new Vector2((float)texture.width / 2, (float)texture.height / 2),
                (float)texture.width / 2);
        }
        
        public static Sprite SpriteFromTexture(Texture2D texture, float pixelsPerUnit)
        {
            if (!IsPower(texture.width, 2.0f) || !IsPower(texture.height, 2.0f))
            {
                Debug.LogWarning($"{texture.name} is not power of 2");
                return null;
            }
            
            
            return Sprite.Create(texture, new Rect(Vector2.zero,
                    new Vector2(texture.width, texture.height)),
                new Vector2((float)texture.width / 2, (float)texture.height / 2),
                pixelsPerUnit);
        }

        public static bool IsPower(float a, float b)
        {
            double res1 = Math.Log(b) / Math.Log(a);
            double res2 = Math.Log(b) / Math.Log(a);

            return (FloatingPointComparision(res1, res2));
        }
        public static bool FloatingPointComparision(double a, double b)
        {
            double diffrence = Math.Abs(a * 0.00001);
            if (Math.Abs(a - b) <= diffrence)
                return true;
            return false;
        }
        public static bool FloatingPointComparision(float a, float b)
        {
            float diffrence = MathF.Abs(a * 0.00001f);
            if (MathF.Abs(a - b) <= diffrence)
                return true;

            return false;
        }

        #region Maths-Floats

        /// <summary>
        /// Round a float value by however many DP (decimal places) default is 2
        /// </summary>
        public static float Round(float value, int dp = 2)
        {
            float mult = Mathf.Pow(10.0f, (float) dp);
            return Mathf.Round(value * mult) / mult;
        }

        public static Vector3 Round(Vector3 v)
        {
            v.x = Round(v.x);
            v.y = Round(v.y);
            if (v.z == 0)
                return v;
            v.z = Round(v.z);
            return v;
        }
        
        #endregion
        

        #region DrawingShapes

        public static void DrawCircle(Vector3 position, float radius, int segments, Color color, float time = 1f)
        {
            if(radius <= 0.0f || segments <= 0)
                return;

            float angleStep = (360.0f / segments);

            angleStep *= Mathf.Deg2Rad;
            
            Vector3 lineStart = Vector3.zero;
            Vector3 lineEnd = Vector3.zero;

            for (int i = 0; i < segments; i++)
            {
                lineStart.x = Mathf.Cos(angleStep * i);
                lineStart.y = Mathf.Sin(angleStep * i);

                lineEnd.x = Mathf.Cos(angleStep * (i + 1));
                lineEnd.y = Mathf.Sin(angleStep * (i + 1));

                lineStart *= radius;
                lineEnd *= radius;

                lineStart += position;
                lineEnd += position;
                
                Debug.DrawLine(lineStart, lineEnd, color, time);
            }
        }

        #endregion
    }
}