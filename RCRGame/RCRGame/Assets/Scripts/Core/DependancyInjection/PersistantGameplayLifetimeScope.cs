using Core.models;
using Core.Services.Gameplay;
using Core3.SciptableObjects;
using DefaultNamespace.Events;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.DependancyInjection
{
    public class PersistantGameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private ColorTileIndex _colorTileIndex;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_colorTileIndex);
            builder.Register<Map>(Lifetime.Singleton);
        }
    }
}