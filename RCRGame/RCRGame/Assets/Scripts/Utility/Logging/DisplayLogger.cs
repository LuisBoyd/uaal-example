using Core3.SciptableObjects;
using TMPro;
using UnityEngine;

namespace Utility.Logging
{
    /// <summary>
    /// Meant for display log text to UI 
    /// </summary>
    public class DisplayLogger : Logger
    {
        private readonly InternalSetting _setting;
        
        public DisplayLogger(ILogHandler logHandler, InternalSetting setting) : base(logHandler)
        {
        }
        
        public void GameLog(LogType logType, string message, TMP_Text text)
        {
            text.text = message;
            text.autoSizeTextContainer = true;
            switch (logType)
            {
                case LogType.Assert:
                    text.color = _setting.DebugAssertColor;
                    break;
                case LogType.Error:
                    text.color = _setting.DebugErrorColor;
                    break;
                case LogType.Exception:
                    text.color = _setting.DebugExceptionColor;
                    break;
                case LogType.Log:
                    text.color = _setting.DebugLogColor;
                    break;
                case LogType.Warning:
                    text.color = _setting.DebugWarningColor;
                    break;

            }
        }
    }
}