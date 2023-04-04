using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace BlackBoard
{
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(fileName = "NewBlackBoard", menuName = "BlackBoard/Board", order = 0)]
    public class Blackboard : ScriptableObject, IDisposable, ICopy<Blackboard>, ISerializationCallbackReceiver
    {
        protected IDictionary<double, int> IntValues = new Dictionary<double, int>();
        protected IDictionary<double, float> FloatValues = new Dictionary<double, float>();
        protected IDictionary<double, bool> BoolValues = new Dictionary<double, bool>();
        protected IDictionary<double, string> StringValues = new Dictionary<double, string>();
        protected IDictionary<double, Vector3> Vector3Values = new Dictionary<double, Vector3>();
        protected IDictionary<double, GameObject> GameObjectValues = new Dictionary<double, GameObject>();
        protected IDictionary<double, object> GenericValues = new Dictionary<double, object>();

        public readonly IDictionary<double, KeyValuePair<string,Type>> fieldName = new Dictionary<double, KeyValuePair<string,Type>>();
        
        protected List<KeyValuePair<double, int>> ListIntValues = new List<KeyValuePair<double, int>>();
        protected List<KeyValuePair<double, float>> ListFloatValues = new List<KeyValuePair<double, float>>();
        protected List<KeyValuePair<double, bool>> ListBoolValues = new List<KeyValuePair<double, bool>>();
        protected List<KeyValuePair<double, string>> ListStringValues = new List<KeyValuePair<double, string>>();
        protected List<KeyValuePair<double, Vector3>> ListVector3Values = new List<KeyValuePair<double, Vector3>>();
        protected List<KeyValuePair<double, GameObject>> ListGameobjectValues = new List<KeyValuePair<double, GameObject>>();
        protected List<KeyValuePair<double, object>> ListGenericValues = new List<KeyValuePair<double, object>>();

        protected List<KeyValuePair<double, KeyValuePair<string, Type>>> ListFieldNames =
            new List<KeyValuePair<double, KeyValuePair<string, Type>>>();

        public void OnBeforeSerialize()
        {
            foreach (var keyValuePair in IntValues)
                ListIntValues.Add(keyValuePair);
            foreach (var keyValuePair in FloatValues)
                ListFloatValues.Add(keyValuePair);
            foreach (var keyValuePair in BoolValues)
                ListBoolValues.Add(keyValuePair);
            foreach (var keyValuePair in StringValues)
                ListStringValues.Add(keyValuePair);
            foreach (var keyValuePair in Vector3Values)
                ListVector3Values.Add(keyValuePair);
            foreach (var keyValuePair in GameObjectValues)
                ListGameobjectValues.Add(keyValuePair);
            foreach (var keyValuePair in GenericValues)
                ListGenericValues.Add(keyValuePair);
            foreach (var keyValuePair in fieldName)
                ListFieldNames.Add(new KeyValuePair<double, KeyValuePair<string, Type>>(keyValuePair.Key, keyValuePair.Value));
        }

        public void OnAfterDeserialize()
        {
            foreach (var keyValuePair in ListIntValues)
                IntValues.Add(keyValuePair);
            foreach (var keyValuePair in ListFloatValues)
                FloatValues.Add(keyValuePair);
            foreach (var keyValuePair in ListBoolValues)
                BoolValues.Add(keyValuePair);
            foreach (var keyValuePair in ListStringValues)
                StringValues.Add(keyValuePair);
            foreach (var keyValuePair in ListVector3Values)
                Vector3Values.Add(keyValuePair);
            foreach (var keyValuePair in ListGameobjectValues)
                GameObjectValues.Add(keyValuePair);
            foreach (var keyValuePair in ListGenericValues)
                GenericValues.Add(keyValuePair);
            foreach (var keyValuePair in ListFieldNames)
                fieldName.Add(new KeyValuePair<double, KeyValuePair<string, Type>>(keyValuePair.Key, keyValuePair.Value));
        }
        
        /// <summary>
        /// Set the value of a key.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public void SetGeneric<T>(string s, T value)
        {
            double key = Hasher.Compute_RollingHash(s);
            SetFieldName(key, s, typeof(T));
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

        public void RemoveGeneric(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            RemoveGeneric(key);
        }
        public void RemoveGeneric(double key)
        {
            if (GenericValues.ContainsKey(key))
            {
                GenericValues.Remove(key);
                fieldName.Remove(key);
            }
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

        public void SetInt(string s, int value)
        {
            double key = Hasher.Compute_RollingHash(s);
            SetFieldName(key, s, typeof(int));
            SetInt(key, value);
        }
        public void SetInt(double key, int value) => Set<int>(IntValues, key, value);
        public int GetInt(string s, int value) => GetInt(Hasher.Compute_RollingHash(s));
        public int GetInt(double key) => Get<int>(IntValues, key);
        public void RemoveInt(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            RemoveInt(key);
        }
        public void RemoveInt(double key)
        {
            if (IntValues.ContainsKey(key))
            {
                IntValues.Remove(key);
                fieldName.Remove(key);
            }
        }

        public void SetFloat(string s, float value)
        {
            double key = Hasher.Compute_RollingHash(s);
            SetFieldName(key, s, typeof(float));
            SetFloat(key, value);
        }
        public void SetFloat(double key, float value) => Set<float>(FloatValues, key, value);
        public float GetFloat(string s, float value) => GetFloat(Hasher.Compute_RollingHash(s));
        public float GetFloat(double key) => Get<float>(FloatValues, key);
        
        public void RemoveFloat(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            RemoveInt(key);
        }
        public void RemoveFloat(double key)
        {
            if (FloatValues.ContainsKey(key))
            {
                FloatValues.Remove(key);
                fieldName.Remove(key);
            }
            
        }

        public void SetBool(string s, bool value)
        {
            double key = Hasher.Compute_RollingHash(s);
            SetFieldName(key, s, typeof(bool));
            SetBool(key, value);
        }
        public void SetBool(double key, bool value) => Set<bool>(BoolValues, key, value);
        public bool GetBool(string s, bool value) => GetBool(Hasher.Compute_RollingHash(s));
        public bool GetBool(double key) => Get<bool>(BoolValues, key);
        
        public void RemoveBool(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            RemoveBool(key);
        }
        public void RemoveBool(double key)
        {
            if (BoolValues.ContainsKey(key))
            {
                BoolValues.Remove(key);
                fieldName.Remove(key);
            }
        }

        public void SetString(string s, string value)
        {
            double key = Hasher.Compute_RollingHash(s);
            SetFieldName(key, s, typeof(string));
            SetString(key, value);
        }
        public void SetString(double key, string value) => Set<string>(StringValues, key, value);
        public string GetString(string s, string value) => GetString(Hasher.Compute_RollingHash(s));
        public string GetString(double key) => Get<string>(StringValues, key);

        public void RemoveString(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            RemoveString(key);
        }
        public void RemoveString(double key)
        {
            if (StringValues.ContainsKey(key))
            {
                StringValues.Remove(key);
                fieldName.Remove(key);
            }
        }
        public void SetVector3(string s, Vector3 value)
        {
            double key = Hasher.Compute_RollingHash(s);
            SetFieldName(key, s, typeof(Vector3));
            SetVector3(Hasher.Compute_RollingHash(s), value);
        }
        public void SetVector3(double key, Vector3 value) => Set<Vector3>(Vector3Values, key, value);
        public Vector3 GetVector3(string s, Vector3 value) => GetVector3(Hasher.Compute_RollingHash(s));
        public Vector3 GetVector3(double key) => Get<Vector3>(Vector3Values, key);
        
        public void RemoveVector3(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            RemoveVector3(key);
        }
        public void RemoveVector3(double key)
        {
            if (Vector3Values.ContainsKey(key))
            {
                Vector3Values.Remove(key);
                fieldName.Remove(key);
            }
            
        }

        public void SetGameobject(string s, GameObject value)
        { 
            double key = Hasher.Compute_RollingHash(s);
            SetFieldName(key, s, typeof(GameObject));
            SetGameobject(Hasher.Compute_RollingHash(s), value);
        }
        public void SetGameobject(double key, GameObject value) => Set<GameObject>(GameObjectValues, key, value);
        public GameObject GetGameobject(string s, GameObject value) => GetGameobject(Hasher.Compute_RollingHash(s));
        public GameObject GetGameobject(double key) => Get<GameObject>(GameObjectValues, key);
        
        public void RemoveGameobject(string s)
        {
            double key = Hasher.Compute_RollingHash(s);
            RemoveGameobject(key);
        }
        public void RemoveGameobject(double key)
        {
            if (GameObjectValues.ContainsKey(key))
            {
                GameObjectValues.Remove(key);
                fieldName.Remove(key);
            }
                
        }
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

        private void SetFieldName(double key, string fieldname, Type type)
        {
            if(!fieldName.ContainsKey(key))
                fieldName.Add(key, new KeyValuePair<string, Type>(fieldname, type));
            else
            {
                Debug.LogWarning($"Can't have multiple fields of the same name {fieldname}");
            }
        }

        private void RemoveFieldName(double key)
        {
            if (fieldName.ContainsKey(key))
                fieldName.Remove(key);
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

        public Blackboard DeepCopy()
        {
            return Instantiate(this);
        }

        public object Clone() => DeepCopy();
   
    }
}