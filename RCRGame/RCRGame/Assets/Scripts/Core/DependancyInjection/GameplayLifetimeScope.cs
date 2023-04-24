using Core.Services.Gameplay;
using Core.Services.Marina;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.DependancyInjection
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private MarinaBuilderService _builderService;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_builderService);
        }
    }
}