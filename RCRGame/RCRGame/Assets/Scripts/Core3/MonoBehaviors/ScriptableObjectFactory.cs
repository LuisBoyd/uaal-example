using System;
using System.Linq;
using Core3.SciptableObjects;
using UnityEngine;

namespace Core3.MonoBehaviors
{
    public static class ScriptableObjectFactory
    {
        private  static AssetDatabaseSO _assetDatabase;

        
        public static void SetAssetDatabase(AssetDatabaseSO databaseSo)
        {
            _assetDatabase = CreateObject(databaseSo);
        }
        
        public static T CreateInstanceOfRandomObject<T>() where T : BaseScriptableObject
        {
            T obj = _assetDatabase.scriptableObjects.OfType<T>()
                .OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            if (obj != null)
            {
                T instance = ScriptableObject.CreateInstance<T>();
                instance.Init(obj);
                return instance;
            }
            return null;
        }

        public static T GetObject<T>(string id) where T : BaseScriptableObject
        {

           return _assetDatabase.scriptableObjects.OfType<T>()
                .Where(x => x.id == Guid.Parse(id)).FirstOrDefault();
        }

        public static T GetObjectAndCreateInstance<T>(string id) where T : BaseScriptableObject
        {
            T obj = _assetDatabase.scriptableObjects.OfType<T>()
                .Where(x => x.id == Guid.Parse(id)).FirstOrDefault();
            if (obj != null)
            {
                T instance = ScriptableObject.CreateInstance<T>();
                instance.Init(obj);
                return instance;
            }
            return null;
        }

        public static T CreateObject<T>(T scriptableObject) where T : BaseScriptableObject
        {
            T instance = ScriptableObject.CreateInstance<T>();
            instance.Init(scriptableObject);
            return instance;
        }
    }
}