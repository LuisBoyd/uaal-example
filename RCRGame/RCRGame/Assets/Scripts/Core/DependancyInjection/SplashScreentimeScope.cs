using Core.Services.Network;
using Core.Services.persistence;
using Core3.SciptableObjects;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using DefaultNamespace.Server;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DefaultNamespace
{
    public class SplashScreentimeScope : LifetimeScope
    {
        [SerializeField] 
        private LoginForm _loginForm;
        [SerializeField] 
        private SceneSO successfulLoginScene;

        [Header("BroadCasting On")]
        [SerializeField] 
        private LoadEventChannelSO SceneLoadSo;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LoginControlFlow>();
            // builder.Register<ILoginService>(resolver =>
            // {
            //     var networkClient = resolver.Resolve<NetworkClient>();
            //     var userSession = resolver.Resolve<User>();
            //     var InternalSettings = resolver.Resolve<InternalSetting>();
            //     var loader = resolver.Resolve<UserLoader>();
            //     return new LoginService(networkClient, userSession, SceneLoadSo, successfulLoginScene, InternalSettings,
            //         loader);
            // }, Lifetime.Singleton);
            builder.Register<ILoginService, LoginService>(Lifetime.Singleton)
                .WithParameter<SceneSO>(successfulLoginScene).WithParameter<LoadEventChannelSO>(SceneLoadSo);
            builder.RegisterComponent(_loginForm);
        }
    }
}