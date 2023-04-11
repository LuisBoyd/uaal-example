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
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LoginControlFlow>();
            builder.Register<ILoginService, LoginService>(Lifetime.Singleton);
            builder.RegisterComponent(_loginForm);
        }
    }
}