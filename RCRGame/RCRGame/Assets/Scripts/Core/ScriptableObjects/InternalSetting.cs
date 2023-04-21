using System;
using DefaultNamespace.Core.Enum;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;
using Utility.Serialization;
using VContainer;
using JsonSerializer = Utility.JsonSerializer;

namespace Core3.SciptableObjects
{

    [CreateAssetMenu(fileName = "New_InternalSetting", menuName = "RCR/Asset/InternalSetting", order = 0)]
    public class InternalSetting : GenericBaseScriptableObject<InternalSetting>
    {
        [ShowInInspector]
        [CanBeNull] public string DBconnectionString { get; set; }
        [ShowInInspector]
        [CanBeNull] public string RootEndPoint { get; set; }

        [EnumToggleButtons] 
        public ContentType WebReqestContentType;

        [CanBeNull][JsonIgnore]
        public string OldestLogPath { get; set; }

     
        [CanBeNull][JsonIgnore]
        public string PreviousLogPath { get; set; } 

        
        [CanBeNull][JsonIgnore]
        public string NewestLogPath { get; set; }
        
        [CanBeNull][JsonIgnore]
        public string UserDataLocalSavePath { get; set; }
        
        [ShowInInspector]
        public int DefaultRequestTimeOut { get; set; } = 2;
        
        [ShowInInspector]
        public int LocalSaveTimeMilliSeconds { get; set; } = 10000; //every 10s default
        
        [ShowInInspector]
        public int RemoteSaveTimeMilliSeconds { get; set; } = 10000; //every 10s default
        
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
            UserDataLocalSavePath = Application.persistentDataPath + "/userdata.txt";
        }

        protected override void Initialize(InternalSetting obj)
        {
            this.DebugAssertColor = obj.DebugAssertColor;
            this.DebugErrorColor = obj.DebugErrorColor;
            this.DebugExceptionColor = obj.DebugExceptionColor;
            this.DebugLogColor = obj.DebugLogColor;
            this.DebugWarningColor = obj.DebugWarningColor;
            this.DBconnectionString = obj.DBconnectionString;
            this.DefaultRequestTimeOut = obj.DefaultRequestTimeOut;
            this.RootEndPoint = obj.RootEndPoint;
            this.NewestLogPath = obj.NewestLogPath;
            this.PreviousLogPath = obj.PreviousLogPath;
            this.OldestLogPath = obj.OldestLogPath;
            this.UserDataLocalSavePath = obj.UserDataLocalSavePath;
            this.WebReqestContentType = obj.WebReqestContentType;
            this.RemoteSaveTimeMilliSeconds = obj.RemoteSaveTimeMilliSeconds;
            this.LocalSaveTimeMilliSeconds = obj.LocalSaveTimeMilliSeconds;
        }
        
        public static InternalSetting CreateInternalSettingInstance(string filepath, IDeserializer<string> deserializer = null)
        {
            InternalSetting instance = ScriptableObject.CreateInstance<InternalSetting>();
            instance.Initialize(deserializer.DeserializeFromPath<InternalSetting>(filepath));
            return instance;
        }

        public string GetContentHeader()
        {
            switch (WebReqestContentType)
            {
                case ContentType.Json:
                    return "application/json";
                    break;
                case ContentType.Text:
                    return "text/plain";
                    break;
                case ContentType.Javascript:
                    return "application/javascript";
                    break;
                case ContentType.Html:
                    return "text/html";
                    break;
                case ContentType.XML:
                    return "application/xml";
                    break;
                default:
                    return string.Empty;
            }
        }

#if UNITY_EDITOR
        //
        // [Button("To Json")]
        // private void SerializeSettings()
        // {
        //     JsonSerializer.Serialize(Application.dataPath, this);
        // }
        //
        // [Button("from Json")]
        // private void DeserializeSettings()
        // {
        //     InternalSetting setting = JsonDeserializer.Deserialize<InternalSetting>();
        //     if (setting == null) return;
        //     this.DebugAssertColor = setting.DebugAssertColor;
        //     this.DebugErrorColor = setting.DebugErrorColor;
        //     this.DebugExceptionColor = setting.DebugExceptionColor;
        //     this.DebugLogColor = setting.DebugLogColor;
        //     this.DebugWarningColor = setting.DebugWarningColor;
        //     this.DBconnectionString = setting.DBconnectionString;
        //     this.DefaultRequestTimeOut = setting.DefaultRequestTimeOut;
        //     this.RootEndPoint = setting.RootEndPoint;
        //     this.NewestLogPath = setting.NewestLogPath;
        //     this.PreviousLogPath = setting.PreviousLogPath;
        //     this.OldestLogPath = setting.OldestLogPath;
        // }
#endif
        
    }
}