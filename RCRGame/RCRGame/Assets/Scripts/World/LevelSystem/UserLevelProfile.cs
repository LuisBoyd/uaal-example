using System.Linq;
using RCRCoreLib.Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace World.LevelSystem
{
    [GlobalConfig("Assets/DataObjects/ConfigFiles/")]
    public class UserLevelProfile : JsonGlobalConfig<UserLevelProfile>
    {
        [ReadOnly] [ListDrawerSettings(ShowFoldout = true)]
        public LevelInfo[] AllLevels;
        

        [HideInInspector]
        public int CurrentUserLevelId;
        [HideInInspector]
        public int CurrentUserLevelexperience;

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateLevelOverview()
        {
            //Finds and assigns all scriptable objects of type LevelInfo
            this.AllLevels = AssetDatabase.FindAssets("t:LevelInfo")
                .Select(guid => AssetDatabase.LoadAssetAtPath<LevelInfo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
#endif
        public override void SerializeConfig()
        {
            throw new System.NotImplementedException();
        }

        public override void DeserializeConfig()
        {
            throw new System.NotImplementedException();
        }
    }
}