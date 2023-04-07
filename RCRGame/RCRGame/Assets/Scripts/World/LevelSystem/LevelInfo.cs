using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace World.LevelSystem
{
    public class LevelInfo : SerializedScriptableObject
    {
        [HorizontalGroup("Row0")]
        [SerializeField]
        public string Name;
        [HorizontalGroup("Row1")]
        [SerializeField]
        public int LevelID;

        [HorizontalGroup("Row2")] [SerializeField]
        public int ExpirenceToLevelUp;
    }
}