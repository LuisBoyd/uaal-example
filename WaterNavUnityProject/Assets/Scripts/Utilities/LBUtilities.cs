﻿using System.Linq;
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
        public const int sortingOrderDefault = 5000;
        
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

        #region Convertes

        public static Vector2 Vector3_To_Vecto2(Vector3 vector)
        {
            return new Vector2(vector.x,vector.y);
        }
        
        #endregion

        /// <summary>
        /// Get which way a slope is going clockwise or counter clockwise (and collinear if it's straight)
        /// </summary>
        /// <returns>
        ///  1 if the slope a -> b -> c is CounterClockWise,
        ///  0 if the slope a -> b -> c is Collinear,
        ///  -1 if the slope a -> b -> c is ClockWise,
        ///
        ///  -2 if something has gone wrong
        /// </returns>
        public static int SlopeOrientation(Vector2 a, Vector2 b, Vector2 c)
        {
            float d = (c.y - b.y) * (b.x - a.x) - (b.y - a.y) * (c.x - b.x);
            if (d > 0)
                return 1;
            if (d < 0)
                return -1;
            else
            {
                return 0;
            }
        }

        #region Topology

        public static Vector2[] graham_Scan(Vector2[] points)
        {
            Vector2 P0 = points.OrderBy(p => p.y).ThenBy(p => p.x).First();
            points = points.OrderBy(p => Mathf.Atan2(p.y - P0.y, p.x - P0.x)).ToArray();
            List<Vector2> hull = new List<Vector2>();
            for (int i = 0; i < points.Length; i++)
            {
                while (hull.Count >= 2 && SlopeOrientation(hull[^2], hull[^1], points[i]) != 1)
                {
                    hull.RemoveAt(hull.Count - 1);
                }
                hull.Add(points[i]);
            }
            return hull.ToArray();
        }
        //Check The order of Vectors and the Slope Orientation
        #endregion

        #region Sorting

        public static T[] HeapSort<T>(T[] array , int size) where T : IComparable
        {
            if (size <= 1)
                return array;

            for (int i = size / 2 - 1; i >= 0; i--)
            {
                Heapify(array, size, i);
            }

            for (int i = size - 1; i >= 0; i--)
            {
                (array[0], array[i]) = (array[i], array[0]);
                //Same as this
                /*  var tempVar = array[0];
                    array[0] = array[i];
                    array[i] = tempVar;
                 */
                Heapify(array, i, 0);
            }

            return array;
        }

        private static void Heapify<T>(T[] array, int size, int index) where T : IComparable
        {
            var largestIndex = index;
            var leftChild = 2 * index + 1;
            var rightChild = 2 * index + 2;

            if (leftChild < size && array[leftChild].CompareTo(array[largestIndex]) > 0)
                largestIndex = leftChild;

            if (rightChild < size && array[rightChild].CompareTo(array[largestIndex]) > 0)
                largestIndex = rightChild;

            if (largestIndex != index)
            {
                var tempvar = array[index];
                array[index] = array[largestIndex];
                array[largestIndex] = tempvar;
                
                Heapify<T>(array,size,largestIndex);
            }
        }
        
        #endregion

        #region InputUtils

        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
        #endregion

        #region TextUtils

        public static void TextPopupMouse(string text, Vector3? offset = null)
        {
            if(offset == null)
                offset = Vector3.one;
            
            CreateWorldTextPopup(text, GetMouseWorldPosition(), 0.1f);
        }
        
        public static void CreateWorldTextPopup(string text, Vector3 localPosition, float popUpTime = 1f)
        {
            CreateWorldTextPopup(null, text, localPosition, 10, Color.white, localPosition + new Vector3(0,20), popUpTime);
        }

        public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize,
            Color color, Vector3 finalPopupPosition, float popupTime)
        {
            TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft,
                TextAlignment.Left, sortingOrderDefault);
            Transform transform = textMesh.transform;
            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(delegate
            {
                transform.position += moveAmount * Time.unscaledTime;
                popupTime -= Time.unscaledDeltaTime;
                if (popupTime <= 0f)
                {
                    UnityEngine.Object.Destroy(transform.gameObject);
                    return true;
                }
                else
                {
                    return false;
                }
            }, "WorldTextPopup");

        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
            Color color, TextAnchor textAnchor,
            TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent,false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        
        #endregion
    }
}