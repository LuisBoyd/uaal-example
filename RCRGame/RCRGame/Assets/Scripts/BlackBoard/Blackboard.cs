using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Hasher;
using UnityEngine;

namespace BlackBoard
{
    [UnityEngine.CreateAssetMenu(fileName = "NewBlackBoard", menuName = "BlackBoard/Board", order = 0)]
    public class Blackboard : ScriptableObject, IDisposable
    {
        protected IDictionary<double, int> IntValues = new Dictionary<double, int>();
        protected IDictionary<double, float> FloatValues = new Dictionary<double, float>();
        protected IDictionary<double, bool> BoolValues = new Dictionary<double, bool>();
        protected IDictionary<double, string> StringValues = new Dictionary<double, string>();
        protected IDictionary<double, Vector3> Vector3Values = new Dictionary<double, Vector3>();
        protected IDictionary<double, GameObject> GameObjectValues = new Dictionary<double, GameObject>();
        protected IDictionary<double, object> GenericValues = new Dictionary<double, object>();

        /// <summary>
        /// Set the value of a key.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public void SetGeneric<T>(string s, T value)
        {
            double key = Hasher.Compute_RollingHash(s);
            SetGeneric<T>(key, value);
        }
        public void SetGeneric<T>(double key, T value)
        {
            GenericValues[key] = value;
        }

        public T GetGeneric<T>(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            return GetGeneric<T>(key);
        }
        public T GetGeneric<T>(double key)
        {
            if (!GenericValues.ContainsKey(key))
                throw new ArgumentException($"Could not find value for {key} in genericValues");

            return (T)GenericValues[key];
        }

        public bool TryGetGeneric<T>(string s, out T value, T defaultValue)
        {
            double d = Hasher.Compute_RollingHash(s);
            return TryGetGeneric<T>(d, out value, defaultValue);
        }
        public bool TryGetGeneric<T>(double key, out T value, T defaultValue)
        {
            if (GenericValues.ContainsKey(key))
            {
                value = (T)GenericValues[key];
                return true;
            }
            value = defaultValue;
            return false;
        }

        public void SetInt(string s, int value) => SetInt(Hasher.Compute_RollingHash(s), value);
        public void SetInt(double key, int value) => Set<int>(IntValues, key, value);
        public int GetInt(string s, int value) => GetInt(Hasher.Compute_RollingHash(s));
        public int GetInt(double key) => Get<int>(IntValues, key);
        
        public void SetFloat(string s, float value) => SetFloat(Hasher.Compute_RollingHash(s), value);
        public void SetFloat(double key, float value) => Set<float>(FloatValues, key, value);
        public float GetFloat(string s, float value) => GetFloat(Hasher.Compute_RollingHash(s));
        public float GetFloat(double key) => Get<float>(FloatValues, key);
        
        public void SetBool(string s, bool value) => SetBool(Hasher.Compute_RollingHash(s), value);
        public void SetBool(double key, bool value) => Set<bool>(BoolValues, key, value);
        public bool GetBool(string s, bool value) => GetBool(Hasher.Compute_RollingHash(s));
        public bool GetBool(double key) => Get<bool>(BoolValues, key);
        
        public void SetString(string s, string value) => SetString(Hasher.Compute_RollingHash(s), value);
        public void SetString(double key, string value) => Set<string>(StringValues, key, value);
        public string GetString(string s, string value) => GetString(Hasher.Compute_RollingHash(s));
        public string GetString(double key) => Get<string>(StringValues, key);
        
        public void SetVector3(string s, Vector3 value) => SetVector3(Hasher.Compute_RollingHash(s), value);
        public void SetVector3(double key, Vector3 value) => Set<Vector3>(Vector3Values, key, value);
        public Vector3 GetVector3(string s, Vector3 value) => GetVector3(Hasher.Compute_RollingHash(s));
        public Vector3 GetVector3(double key) => Get<Vector3>(Vector3Values, key);
        
        public void SetGameobject(string s, GameObject value) => SetGameobject(Hasher.Compute_RollingHash(s), value);
        public void SetGameobject(double key, GameObject value) => Set<GameObject>(GameObjectValues, key, value);
        public GameObject GetGameobject(string s, GameObject value) => GetGameobject(Hasher.Compute_RollingHash(s));
        public GameObject GetGameobject(double key) => Get<GameObject>(GameObjectValues, key);
        private void Set<T>(IDictionary<double, T> keySet, double key, T value)
        {
            if (!keySet.ContainsKey(key))
            {
                keySet.Add(key, value);
            }
            keySet[key] = value;
        }
        private T Get<T>(IDictionary<double, T> keySet, double key)
        {
            if (!keySet.ContainsKey(key))
                throw new ArgumentException($"Could not find value for {key} in {typeof(T).Name} collection");
            return keySet[key];
        }

        public void Dispose()
        {
            IntValues.Clear();
            FloatValues.Clear();
            BoolValues.Clear();
            StringValues.Clear();
            Vector3Values.Clear();
            GameObjectValues.Clear();
            GenericValues.Clear();
        }
    }
}