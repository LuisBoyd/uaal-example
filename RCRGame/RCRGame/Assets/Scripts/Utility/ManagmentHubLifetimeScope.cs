using Core.Services.Network;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using UI;
using UI.RecyclableScrollRect;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utility
{
    public class ManagmentHubLifetimeScope : LifetimeScope
    {
        [SerializeField] 
        private RecyclableMarinaView _recyclableMarinaView;
        [SerializeField]
        private EventRelay OnNewMarinaDataSet;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MarianaCollection>(Lifetime.Singleton)
                .WithParameter<EventRelay>(OnNewMarinaDataSet).AsSelf();
            builder.RegisterInstance(OnNewMarinaDataSet);
            // builder.Register<MarianaCollection>(Lifetime.Singleton)
            //     .WithParameter<EventRelay>(OnNewMarinaDataSet);
            builder.RegisterComponent(_recyclableMarinaView);
        }
    }
}