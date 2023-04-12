using UnityEngine;

namespace Utility.Logging
{
    public class RuntimeLogger : Logger
    {
        public delegate void RuntimeLog(string message, LogType logType);

        public event RuntimeLog OnRuntimeLog;
            
        public RuntimeLogger( RuntimeLogHandler LogHandler) : base(LogHandler)
        {
            Application.logMessageReceived += ApplicationOnlogMessageReceived;
        }

        private void ApplicationOnlogMessageReceived(string condition, string stacktrace, LogType type)
        {
           OnRuntimeLog?.Invoke(condition, type);
        }
    }
}