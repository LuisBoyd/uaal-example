using System;
using System.IO;
using Core3.SciptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utility.Logging
{
    public class RuntimeLogHandler : ILogHandler
    {
        private readonly InternalSetting _setting;
        private ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;

        private FileStream m_fileStream;
        private StreamWriter m_StreamWriter;
        private readonly bool LogToFile = true;

        public RuntimeLogHandler(InternalSetting settings)
        {
            _setting = settings;
            if (File.Exists(_setting.OldestLogPath) && LogToFile)
            {
                try
                {
                    File.Delete(_setting.OldestLogPath);
                }
                catch (Exception e)
                {
                    LogToFile = false;
                    Debug.LogError($"Problem Occured while setting up Logger can't log to files");
                }
            }
            if (File.Exists(_setting.PreviousLogPath) && LogToFile)
            {
                try
                {
                    File.Move(_setting.PreviousLogPath, _setting.OldestLogPath);
                }
                catch (Exception e)
                {
                    LogToFile = false;
                    Debug.LogError($"Problem Occured while setting up Logger can't log to files");
                }
            }
            if (File.Exists(_setting.NewestLogPath) && LogToFile)
            {
                try
                {
                    File.Move(_setting.NewestLogPath, _setting.PreviousLogPath);
                }
                catch (Exception e)
                {
                    LogToFile = false;
                    Debug.LogError($"Problem Occured while setting up Logger can't log to files");
                }
            }
            //Now The Newest Log Path should not exist.
            if (!File.Exists(_setting.NewestLogPath) && LogToFile)
            {
                try
                {
                    m_fileStream = File.Create(_setting.NewestLogPath);
                    m_StreamWriter = new StreamWriter(m_fileStream);
                }
                catch (Exception e)
                {
                    LogToFile = false;
                    Debug.LogError($"Problem Occured while setting up Logger can't log to files");
                }
            }
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            string message = String.Format(format, args);
            m_StreamWriter.WriteLine(message);
            m_StreamWriter.Flush();
            m_DefaultLogHandler.LogFormat(logType,context,format,args);
            
        }
        public void LogException(Exception exception, Object context)
        {
            string message =
                $"[{DateTime.Now.ToLongTimeString()}][{exception.GetType().ToString()}] {exception.Message} \n" +
                $"Source: {exception.Source}";
            m_StreamWriter.WriteLine(message);
            m_StreamWriter.Flush();
            m_DefaultLogHandler.LogException(exception,context);
        }
        
    }
}