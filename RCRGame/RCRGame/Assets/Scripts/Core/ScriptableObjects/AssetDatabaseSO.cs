using UnityEngine;

namespace Core3.SciptableObjects
{
    [CreateAssetMenu(fileName = "New_AssetDatabaseSO", menuName = "RCR/Asset/AssetDatabase", order = 0)]
    public class AssetDatabaseSO : GenericBaseScriptableObject<AssetDatabaseSO>
    {
        public BaseScriptableObject[] scriptableObjects;
        
        protected override void Initialize(AssetDatabaseSO obj)
        {
            scriptableObjects = obj.scriptableObjects;
        }
        
    }
}