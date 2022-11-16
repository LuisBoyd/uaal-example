using System;
using System.Collections.Generic;
using RCR.Enums;
using UnityEngine;

namespace RCR.Utilities
{
    public static class RiverCanalLogger
    {

        public static readonly Dictionary<RCRMessageType, string> Messages = new Dictionary<RCRMessageType, string>()
        {
            { RCRMessageType.MAP_NOT_EQUIDIMENSIONAL, "Gathered Map is not Equidimensional"},
            { RCRMessageType.MAP_PROCESSING_PROBLEM , "Problem with Processing map"}
        };

        public class RCRMessage
        {
            public RCRSeverityLevel Severity;
            public string Message;
            public Exception Exception;

            public RCRMessage(RCRSeverityLevel severity, RCRMessageType messageType, Exception e = null)
            {
                this.Severity = severity;
                this.Message = RiverCanalLogger.Messages[messageType];
                this.Exception = e;
            }
        }
        
        

        public static void Log(RCRMessage message)
        {
            switch (message.Severity)
            {
                case RCRSeverityLevel.LOG:
                    Debug.Log(message);
                    break;
                case RCRSeverityLevel.ERROR:
                    Debug.LogError(message);
                    break;
                case RCRSeverityLevel.ASSERT:
                    Debug.LogAssertion(message);
                    break;
                case RCRSeverityLevel.WARNING:
                    Debug.LogWarning(message);
                    break;
                case RCRSeverityLevel.EXCEPTION:
                    Debug.LogException(message.Exception);
                    break;
            }
        }
    }
}