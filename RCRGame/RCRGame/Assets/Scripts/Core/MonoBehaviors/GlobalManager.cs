using System;
using Core3.SciptableObjects;

namespace Core3.MonoBehaviors
{
    public class GlobalManager : BaseManager
    {
        public AssetDatabaseSO assetDatabase;
        

        protected virtual void Start()
        {
            ScriptableObjectFactory.SetAssetDatabase(assetDatabase);
        }
    }
}