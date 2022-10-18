using Newtonsoft.Json.Linq;
using RCR.BaseClasses;
using UnityEngine;
using UnityEngine.UI;

namespace RCR.Utilities
{
    public class LoggingUtils: Singelton<LoggingUtils>
    {
        [SerializeField] private Text m_textField;
        protected override void Awake()
        {
            base.Awake();
            NativeBridge.Instance.InsertCMD("Debug", RecieveJsonMessage);
        }

        void appendToText(string line) { m_textField.text += line + "\n"; }

        public void RecieveJsonMessage(string cmd)
        {
            JObject cmdJsonLog = JObject.Parse(cmd);
            if (cmdJsonLog != null)
            {
                string message = cmdJsonLog["message"].ToString();
                Debug(message);
            }
        }
        
        public void Debug(string message)
        {
            appendToText(message);
        }
    }
}