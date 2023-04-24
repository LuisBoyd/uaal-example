using Core.Services.Gameplay;
using Core.Services.Network;
using Core.Services.persistence;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using DefaultNamespace.Tests;
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

        [SerializeField] private MoneyIncreaserTest _increaserTest;

        [SerializeField] private MarinaBuyService _marinaBuyService;
        [SerializeField] private EventRelay OnSuccessfulPurchaseChannel;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MarianaCollection>(Lifetime.Singleton)
                .WithParameter("newMarinaSetNotifier",OnNewMarinaDataSet)
                .WithParameter("onSuccessfulBuyEvent",OnSuccessfulPurchaseChannel).AsSelf();
            builder.RegisterComponent(_marinaBuyService);
            builder.RegisterEntryPoint<UserSaver>(Lifetime.Singleton);
            builder.RegisterInstance(OnNewMarinaDataSet);
            // builder.Register<MarianaCollection>(Lifetime.Singleton)
            //     .WithParameter<EventRelay>(OnNewMarinaDataSet);
            builder.RegisterComponent(_recyclableMarinaView);
            builder.RegisterComponent(_increaserTest);
        }
    }
}