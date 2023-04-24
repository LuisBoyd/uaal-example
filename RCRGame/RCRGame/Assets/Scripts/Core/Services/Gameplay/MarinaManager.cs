using System;
using Core.Services.persistence;
using Core3.MonoBehaviors;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Core.Services.Gameplay
{
    public class MarinaManager : BaseMonoBehavior
    {
        #region InjectedValues
        private MarinaTileCreator _marinaTileCreator;
        [Inject]
        private void InjectValues(MarinaTileCreator marinaTileCreator)
        {
            _marinaTileCreator = marinaTileCreator;
        }
        #endregion

        #region fields
        [Header("Fields")] 
        [SerializeField] private Tilemap IsometricBaseMap;
        #endregion
        
        private async UniTaskVoid Start()
        {
            await _marinaTileCreator.CreateTilemap(IsometricBaseMap);
        }
    }
}