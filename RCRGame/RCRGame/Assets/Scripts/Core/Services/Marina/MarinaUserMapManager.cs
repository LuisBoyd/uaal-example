using System;
using Core.models;
using Core.Services.Network;
using Core.Services.persistence;
using Core3.MonoBehaviors;
using Core3.SciptableObjects;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Core.models;
using RuntimeModels;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Core.Services.Marina
{
    public class MarinaUserMapManager : BaseMonoBehavior
    {
        //[Title("Listening To Events", TitleAlignment =  TitleAlignments.Centered)]
        //[Title("Broadcasting On Events", TitleAlignment =  TitleAlignments.Centered)]

        [Title("Configuration", TitleAlignment = TitleAlignments.Centered)] 
        [SerializeField] private Tilemap IsometricTileMap;
        [SerializeField] private Tilemap outOfViewTilemap;

        private RuntimeUserMap UserMap;
        
        #region Injected
        private MarinaBuildPipeline BuildPipeline { get; set; }
        private UserMariana UserMariana { get; set; }
        
        
        [Inject]
        private void InjectValues(MarinaBuildPipeline buildPipeline, UserMariana userMariana)
        {
            BuildPipeline = buildPipeline;
            UserMariana = userMariana;
        }
        #endregion

        private async void Start()
        {
            UserMap = await BuildPipeline.BuildMarina(IsometricTileMap,outOfViewTilemap,
                UserMariana.marinaId, UserMariana.system_user_id,
                this.GetCancellationTokenOnDestroy());
            if (UserMap != null)
            {
                Debug.Log("Was successfully built");
            }
            
        }
    }
}