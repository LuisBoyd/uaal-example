using Core.Services;
using Core.Services.Test;
using DefaultNamespace.Tests;
using VContainer;
using VContainer.Unity;

    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(this.gameObject);
            builder.Register<IHttpClient, HttpClient>(Lifetime.Singleton); //Register HTTPClient service
        }
    }