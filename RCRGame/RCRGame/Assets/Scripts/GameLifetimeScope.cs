using System;
using System.IO;
using Core.Services;
using Core.Services.Test;
using Core3.SciptableObjects;
using DefaultNamespace.Tests;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Windows;
using Utility;
using Utility.Logging;
using VContainer;
using VContainer.Unity;
using ILogger = UnityEngine.ILogger;

public class GameLifetimeScope : LifetimeScope
{
        private ILogHandler defaultUnityLogHandler;
        private RuntimeLogHandler _runtimeLogHandler;
        
        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(this.gameObject);
            InternalSetting internalSetting;
            //internalSetting = ObjectDeserializer.Deserialize<InternalSetting>(fullPath);
            // if (internalSetting == null)
            //     throw new Exception("Missing Internal Settings");
            // RuntimeLogger logger = new RuntimeLogger(new RuntimeLogHandler(internalSetting));
            // Debug.unityLogger.logHandler = new RuntimeLogHandler()
            // DisplayLogger displayLogger = new DisplayLogger(Debug.unityLogger.logHandler, internalSetting);

            builder.Register<ISerializer<string>, JsonSerializer>(Lifetime.Singleton);
            builder.Register<IDeserializer<string>, JsonDeserializer>(Lifetime.Singleton);
            
            var fullPath = Path.Combine(Application.dataPath, "Secrets/AppSettings.json");
            internalSetting = InternalSetting.CreateInternalSettingInstance(fullPath, new JsonDeserializer());
            if (internalSetting == null)
                throw new Exception("Missing Internal Settings");
            _runtimeLogHandler = new RuntimeLogHandler(internalSetting);
            defaultUnityLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = _runtimeLogHandler; //Set default logger
            DisplayLogger displayLogger = new DisplayLogger(Debug.unityLogger.logHandler, internalSetting);
            RuntimeLogger runtimeLogger = new RuntimeLogger(_runtimeLogHandler);
            
            builder.RegisterInstance<DisplayLogger>(displayLogger);
            builder.RegisterInstance<RuntimeLogger>(runtimeLogger);
            builder.RegisterInstance<InternalSetting>(internalSetting);
            builder.Register<IHttpClient, HttpClient>(Lifetime.Singleton); //Register HTTPClient service
        }

        private void OnDisable()
        {
            _runtimeLogHandler.Dispose();
        }

        protected override void OnDestroy()
        {
            Debug.unityLogger.logHandler = defaultUnityLogHandler;
            base.OnDestroy();
        }
    }