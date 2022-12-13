using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace RCR.editor
{
    public static class EditorUtilities
    {
        /// <summary>
        /// Check is the serialized property is a refrence type of another class type
        /// </summary>
        /// <returns>Out the Typed Property, returns true or false based on if successful or not</returns>
        public static bool IsSerializedPropertyX<T>(SerializedProperty property, out T T_property) where T : class
        {
            T_property = default;
            if (property.objectReferenceValue is T)
            {
                T_property = property.objectReferenceValue as T;
                return true;
            }

            return false;
        }

        public static bool IsSerializedPropertyOfRefrenceType<T>(SerializedProperty property, out T T_property)
            where T : class
        {
            T_property = default;
            if (property.boxedValue is T)
            {
                T_property = property.boxedValue as T;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Takes in a serializedProperty and a obj refrence value and tries to assign the value to the serializedProperty
        ///
        /// TODO: test the stability of this function
        /// </summary>
        /// <returns>True if operation was successful</returns>
        public static bool SaveRefrenceValueToSerializedProperty(ref SerializedProperty property, Object obj)
        {
            Type typeOfProperty = property.objectReferenceValue.GetType();
            Type objType = obj.GetType();
            if (typeOfProperty.IsAssignableFrom(objType))
            {
                property.objectReferenceValue = obj;
                return true;
            }
            return false;
        }
    }
}