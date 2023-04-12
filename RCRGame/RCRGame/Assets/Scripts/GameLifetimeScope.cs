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
        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(this.gameObject);
            InternalSetting internalSetting;
            var fullPath = Path.Combine(Application.dataPath, "Secrets/AppSettings.json");
            internalSetting = ObjectDeserializerCreator.Deserialize<InternalSetting>(fullPath, null);
            if (internalSetting == null)
                throw new Exception("Missing Internal Settings");
            RuntimeLogger logger = new RuntimeLogger(new RuntimeLogHandler(internalSetting));
            DisplayLogger displayLogger = new DisplayLogger(Debug.unityLogger.logHandler, internalSetting);

            builder.RegisterInstance<DisplayLogger>(displayLogger);
            builder.RegisterInstance<RuntimeLogger>(logger);
            builder.RegisterInstance<InternalSetting>(internalSetting);
            builder.Register<IHttpClient, HttpClient>(Lifetime.Singleton); //Register HTTPClient service
        }
    }