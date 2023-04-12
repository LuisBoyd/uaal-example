using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;
using Utility.Serialization;

namespace Core3.SciptableObjects
{
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New_InternalSetting", menuName = "RCR/Asset/InternalSetting", order = 0)]
#endif
    public class InternalSetting : BaseScriptableObject
    {
        [ShowInInspector]
        [CanBeNull] public string DBconnectionString { get; set; }
        [ShowInInspector]
        [CanBeNull] public string RootEndPoint { get; set; }

       
        [CanBeNull][JsonIgnore]
        public string OldestLogPath { get; set; }

     
        [CanBeNull][JsonIgnore]
        public string PreviousLogPath { get; set; } 

        
        [CanBeNull][JsonIgnore]
        public string NewestLogPath { get; set; }
        
        [ShowInInspector]
        public int DefaultRequestTimeOut { get; set; } = 2;
        
        [ShowInInspector][JsonConverter(typeof(ColorHandler))]
        public Color DebugErrorColor { get; set; } = Color.white;
        [ShowInInspector][JsonConverter(typeof(ColorHandler))]
        public Color DebugAssertColor { get; set; } = Color.white;
        [ShowInInspector][JsonConverter(typeof(ColorHandler))]
        public Color DebugWarningColor { get; set; } = Color.white;
        [ShowInInspector][JsonConverter(typeof(ColorHandler))]
        public Color DebugLogColor { get; set; } = Color.white;
        [ShowInInspector][JsonConverter(typeof(ColorHandler))]
        public Color DebugExceptionColor { get; set; } = Color.white;

        private void OnEnable()
        {
            NewestLogPath = Application.persistentDataPath + "/NewestLog.txt";
            PreviousLogPath = Application.persistentDataPath + "/PreviousLog.txt";
            OldestLogPath = Application.persistentDataPath + "/OldestLog.txt";
        }


#if UNITY_EDITOR
        
        [Button("To Json")]
        private void SerializeSettings()
        {
            ObjectSerializerCreator.ShowDialog(Application.dataPath, this, (setting) =>
            {
                Debug.Log("Setting's where saved");
            });
        }

        [Button("Refresh Default Values")]
        private void RefreshDefaultValue()
        {
            DebugErrorColor = Color.white;
            DebugAssertColor = Color.white;
            DebugWarningColor = Color.white;
            DebugLogColor = Color.white;
            DebugExceptionColor = Color.white;
            DefaultRequestTimeOut = 2;
        }
#endif
        
    }
}