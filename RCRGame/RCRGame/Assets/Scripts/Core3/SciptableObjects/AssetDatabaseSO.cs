using UnityEngine;

namespace Core3.SciptableObjects
{
    [CreateAssetMenu(fileName = "New_AssetDatabaseSO", menuName = "RCR/Asset/AssetDatabase", order = 0)]
    public class AssetDatabaseSO : BaseScriptableObject
    {
        public BaseScriptableObject[] scriptableObjects;

        public override void Init(BaseScriptableObject scriptableObject)
        {
            base.Init(scriptableObject);

            if (scriptableObject is AssetDatabaseSO c)
            {
                scriptableObjects = c.scriptableObjects;
            }
        }
    }
}