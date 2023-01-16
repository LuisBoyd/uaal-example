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
    }
}