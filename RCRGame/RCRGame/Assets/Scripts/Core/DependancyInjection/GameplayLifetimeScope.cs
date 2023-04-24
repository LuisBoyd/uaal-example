using Core.Services.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.DependancyInjection
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private MarinaManager _marinaManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_marinaManager);
        }
    }
}