using Core.Services.Gameplay;
using Core.Services.Marina;
using RuntimeModels;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace Core.DependancyInjection
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [FormerlySerializedAs("_builderService")] [SerializeField] private MarinaUserMapManager userMapManager;
        

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(userMapManager);
        }
    }
}