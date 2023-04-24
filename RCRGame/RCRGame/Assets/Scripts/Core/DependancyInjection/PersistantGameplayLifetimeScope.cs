using Core.models;
using Core.Services.Gameplay;
using Core.Services.Marina;
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
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(ColorTileIndex);
            builder.Register<MarinaBase64ConverterToTex2D>(resolver => 
                new MarinaBase64ConverterToTex2D(MarinaPlotTextureResolution.x, MarinaPlotTextureResolution.y,
                    TextureFormat.ASTC_6x6), Lifetime.Singleton);
            builder.Register<MarinaTexture2DConverterToTiles>(Lifetime.Singleton);
        }
    }
}