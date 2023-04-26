using Core.Camera;
using Core.models;
using Core.Services.Gameplay;
using Core.Services.Marina;
using Core.Services.Network;
using Core3.SciptableObjects;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.DependancyInjection
{
    public class PersistantGameplayLifetimeScope : LifetimeScope
    {
        [Title("Configurations", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField]private Vector2Int MarinaPlotTextureResolution;
        [SerializeField] private ColorTileIndex ColorTileIndex;
        [SerializeField] private CameraRestrictions CameraRestriction;
        [SerializeField] private PolygonCollider2D polygonCollider2D;
        
        protected override void Configure(IContainerBuilder builder)
        {
          
            //builder.RegisterComponent(CameraRestriction);
            builder.Register<MarinaBuildPipeline>(resolver =>
            {
                InternalSetting setting = resolver.Resolve<InternalSetting>();
                NetworkClient networkClient = resolver.Resolve<NetworkClient>();
                UserMariana userMariana = resolver.Resolve<UserMariana>();
                return new MarinaBuildPipeline(setting, networkClient,userMariana,
                    new ApplyCameraRestriction(polygonCollider2D),
                    new ApplyTilesDecorator(ColorTileIndex,
                        MarinaPlotTextureResolution.x, MarinaPlotTextureResolution.y),
                    new LoadDecorator(networkClient, setting));
            }, Lifetime.Singleton);
        }
    }
}