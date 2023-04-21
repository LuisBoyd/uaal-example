using System;
using System.IO;
using Core.Services;
using Core.Services.Network;
using Core3.SciptableObjects;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using UnityEngine;
using Utility.Logging;
using VContainer;
using VContainer.Unity;

namespace Utility
{
    public class AppLifetimeScope : LifetimeScope
    {
        private InternalSetting _setting;
        private ILogHandler _defaultUnityLogHandler;
        private RuntimeLogHandler _runtimeLogHandler;
        private JsonDeserializer _jsonDeserializer;
        private JsonSerializer _jsonSerializer;
        private DisplayLogger _displayLogger;
        private NetworkClient _networkClient;
        private IProgress<float> _progressReporter;

        [SerializeField] private SceneSO _loginFormSceneSo;
        [SerializeField] private LoadEventChannelSO _loadSceneEvent;
        [SerializeField] private InfoDisplayEventChannelSO _visualInfoLoggerEvent;
        [SerializeField] private TextAsset PreCompiledSetting; //In actual production take this out.

        protected override void Configure(IContainerBuilder builder)
        {
            _jsonDeserializer = new JsonDeserializer();
            _jsonSerializer = new JsonSerializer();
            
            _setting = _jsonDeserializer.Deserialize<InternalSetting>(PreCompiledSetting.text);
            if (_setting == null) throw new NullReferenceException("Missing Setting Object");
            _runtimeLogHandler = new RuntimeLogHandler(_setting);
            _defaultUnityLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = _runtimeLogHandler;
            _displayLogger = new DisplayLogger(_runtimeLogHandler, _setting);
            _progressReporter = new UnityProgressReport();
            // _networkClient = new NetworkClient(_setting.RootEndPoint,
            //     TimeSpan.FromSeconds(_setting.DefaultRequestTimeOut),
            //     _progressReporter,
            //     new LoggingDecorator(),
            //     new ReturnToLoginPageDecorator());


            builder.Register<NetworkClient>(resolver => new NetworkClient(_setting,
                TimeSpan.FromSeconds(_setting.DefaultRequestTimeOut),
                _progressReporter,
                new LoggingDecorator(_visualInfoLoggerEvent),
                new SetupHeaderDecorator(_setting),
                new ReturnToLoginPageDecorator(_loginFormSceneSo, _loadSceneEvent),
                new AuthenticationDecorator()),
                Lifetime.Singleton);
            
            builder.Register<User>(Lifetime.Singleton);
            builder.RegisterInstance<ISerializer<string>>(_jsonSerializer);
            builder.RegisterInstance<IDeserializer<string>>(_jsonDeserializer);
            //builder.RegisterInstance<NetworkClient>(_networkClient);
            builder.RegisterInstance<IProgress<float>>(_progressReporter);
            builder.RegisterInstance<DisplayLogger>(_displayLogger);
            builder.RegisterInstance<InternalSetting>(_setting);
        }

        private void OnDisable()
        {
            _runtimeLogHandler.Dispose();
            Debug.unityLogger.logHandler = _defaultUnityLogHandler;
        }
    }
}