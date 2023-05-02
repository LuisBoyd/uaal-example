using System;
using Core3.MonoBehaviors;
using Core3.SciptableObjects;
using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace Core.Services.Gameplay
{
    public class TilemapService : BaseMonoBehavior
    {
        [Title("Tilemap Service Configuration", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField][Required]private  ColorTileIndex _colorTileIndex;
        [SerializeField][Required]private  BoolEventChannelSO _serviceSwitchEventChannel;
        [SerializeField][Required]private  BoolEventChannelSO _cameraStateChangeEvent;
        [SerializeField][Required]private  Tilemap _isometricWorldTilemap;
        [SerializeField][Required]private  TilemapRenderer _isometricWorldTilemapRenderer;

        private LocalKeyword OutlineEnabledKeyWord;
        private const string KEYWORD_SHADER = "_OUTLINE_ENABLED";
        
        private Material GridOutlineMaterial
        {
            get
            {
                //Returns back the Copy Material so if any other objects use the same
                //Matireal any Changes I make here will not happen to all gameObjects with
                //This Matireal only the gameObject this renderer is attached to.
                return _isometricWorldTilemapRenderer.material;
            }
        }

        private void Awake()
        {
            OutlineEnabledKeyWord = new LocalKeyword(GridOutlineMaterial.shader, KEYWORD_SHADER);
        }

        private void OnEnable()
        {
            _serviceSwitchEventChannel.onEventRaised += OnServiceStateSwitched;
            GridOutlineMaterial.SetKeyword(OutlineEnabledKeyWord, false);
        }

        private void OnDisable()
        {
            _serviceSwitchEventChannel.onEventRaised -= OnServiceStateSwitched;
            GridOutlineMaterial.SetKeyword(OutlineEnabledKeyWord, false);
        }

        private void OnServiceStateSwitched(bool value)
        {
            GridOutlineMaterial.SetKeyword(OutlineEnabledKeyWord, value);
            _cameraStateChangeEvent.RaiseEvent(!value);
        }
    }
}