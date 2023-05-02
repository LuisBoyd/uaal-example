using Cinemachine;
using Core.Camera;
using Core.Entity;
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
        [SerializeField] private PolygonCollider2D polygonCollider2D;
        [SerializeField] private PolygonCollider2D outsidePolygon2D;
        [SerializeField] private Transform CameraFollower;
        [SerializeField] private CinemachineConfiner2D _cinemachineConfiner2D;
        [SerializeField] private CameraSystem _cameraSystem;
        [SerializeField] private AstarPath _astarPath;
        [SerializeField] private BoatSpawner _narrowBoatSpawner;
        
        protected override void Configure(IContainerBuilder builder)
        {
          
            //builder.RegisterComponent(CameraRestriction);
            builder.Register<MarinaBuildPipeline>(resolver =>
            {
                InternalSetting setting = resolver.Resolve<InternalSetting>();
                NetworkClient networkClient = resolver.Resolve<NetworkClient>();
                UserMariana userMariana = resolver.Resolve<UserMariana>();
                return new MarinaBuildPipeline(setting, networkClient,userMariana,
                    new ApplyCameraRestriction(polygonCollider2D, outsidePolygon2D ,CameraFollower, _cinemachineConfiner2D),
                    new ApplyAStarGrid(_astarPath),
                    new ApplyTilesDecorator(ColorTileIndex,
                        MarinaPlotTextureResolution.x, MarinaPlotTextureResolution.y),
                    new LoadDecorator(networkClient, setting));
            }, Lifetime.Singleton);
            builder.RegisterComponent(_cameraSystem);
            builder.RegisterComponent(_narrowBoatSpawner);
        }
    }
}