using System;
using Core.models;
using Core.Services.Network;
using Core.Services.persistence;
using Core3.MonoBehaviors;
using Core3.SciptableObjects;
using DefaultNamespace.Core.models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Core.Services.Marina
{
    public class MarinaBuilderService : BaseMonoBehavior
    {
        //[Title("Listening To Events", TitleAlignment =  TitleAlignments.Centered)]
        //[Title("Broadcasting On Events", TitleAlignment =  TitleAlignments.Centered)]

        [Title("Configuration", TitleAlignment = TitleAlignments.Centered)] 
        [SerializeField] private Tilemap IsometricTileMap;
        

        #region Injected
        private MarinaBase64ConverterToTex2D Base64ConverterToTex2D { get; set; }
        private MarinaTexture2DConverterToTiles Texture2DConverterToTiles { get; set; }
        private NetworkClient NetworkClient { get; set; }
        private UserMariana UserMariana { get; set; }
        private InternalSetting InternalSetting { get; set; }
        [Inject]
        private void InjectValues(MarinaBase64ConverterToTex2D marinaBase64ConverterToTex2D, MarinaTexture2DConverterToTiles marinaTexture2DConverterToTiles,
            UserMariana userMariana, NetworkClient networkClient, InternalSetting setting)
        {
            Base64ConverterToTex2D = marinaBase64ConverterToTex2D;
            Texture2DConverterToTiles = marinaTexture2DConverterToTiles;
            NetworkClient = networkClient;
            UserMariana = userMariana;
            InternalSetting = setting;
        }
        #endregion

        private async void Start()
        {
            UserMapLoader userMapLoader =
                new UserMapLoader(UserMariana, InternalSetting.UserMapDataLocalSavePath, NetworkClient);
            UserMap mostRecentMap = await userMapLoader.LoadMostRecent();
            foreach (Plot plot in mostRecentMap.Plots)
            {
                Texture2D plotTexture = Base64ConverterToTex2D.Convert(plot.Tile_Data);
                if (plotTexture.width != Base64ConverterToTex2D.SetWidth
                    || plotTexture.height != Base64ConverterToTex2D.SetHeight)
                {
                    Debug.LogWarning("Failed to load Marina texture map");
                    return;
                }
                TileBase[] tiles = Texture2DConverterToTiles.Convert(plotTexture);
                if (tiles.Length != Mathf.FloorToInt(Mathf.Pow(plotTexture.width, 2)))
                {
                    Debug.LogWarning("Failed to load Marina texture map");
                    return;
                }
                Vector3Int minPoint = new Vector3Int(plot.Plot_index * plotTexture.width, plot.Plot_index * plotTexture.height,1);
                Vector3Int maxPoint = new Vector3Int(minPoint.x + plotTexture.width, minPoint.y + plotTexture.height,1);
                Vector3Int size = new Vector3Int(maxPoint.x - minPoint.x, maxPoint.y - minPoint.y, 1);
                BoundsInt area = new BoundsInt(minPoint, size);
                IsometricTileMap.SetTilesBlock(area, tiles);
            }
        }
    }
}