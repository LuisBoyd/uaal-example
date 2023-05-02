using System;
using System.Collections.Generic;
using System.Linq;
using Core3.MonoBehaviors;
using DefaultNamespace.Events;
using deVoid.UIFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIArchitecture
{
    public class UIController : BaseMonoBehavior
    {
        [Title("UI Controller Configurations", TitleAlignment = TitleAlignments.Centered)] 
        [Required] [SerializeField] private UISettings defaultUISettings = null;
        [Required] [SerializeField] private EventRelay OngameHudBegin;
        [Required] [SerializeField] private UIDisplayEvent NavigationEventChannel;
        [SerializeField] private float MainCanvasDistance = 5f;

        private Camera _camera;
        private UIFrame _uiFrame;
        
        
        private void Awake()
        {
            _uiFrame = defaultUISettings.CreateUIInstance();
            _camera = Camera.main;
            _uiFrame.MainCanvas.worldCamera = _camera;
            _uiFrame.MainCanvas.planeDistance = MainCanvasDistance;
        }

        private void OnEnable()
        {
            OngameHudBegin.onEventRaised += OnGameHudBegin;
            NavigationEventChannel.onEventRaised += OnNavigateToWindow;
        }

        private void OnDisable()
        {
            OngameHudBegin.onEventRaised -= OnGameHudBegin;
            NavigationEventChannel.onEventRaised -= OnNavigateToWindow;
        }

        private void OnGameHudBegin()
        {
            _uiFrame.ShowPanel(ScreenIds.ConstructionPannel);
            _uiFrame.ShowPanel(ScreenIds.StatInfoPanel);
            _uiFrame.ShowPanel(ScreenIds.MenuPanel);
        }

        private void Start()
        {
            _uiFrame.OpenWindow(ScreenIds.GameHUDWindow);
        }

        private void OnNavigateToWindow(bool closeWindow,string windowID)
        {
            if (string.IsNullOrEmpty(windowID))
            {
                Debug.LogWarning($"Window Id is Null or Empty");
                return;
            }

            if (closeWindow)
            {
                _uiFrame.CloseCurrentWindow();
            }
            switch (windowID)
            {
                case ScreenIds.GameHUDWindow:
                    Manage_Game_HUD_Display();
                    break;
                case ScreenIds.TileEditingPanel:
                    //BuildingConstruction so all that.
                    Manage_Tile_Editing_Display();
                    break;
            }
        }

        private void Manage_Game_HUD_Display()
        {
            //hide the unnecessary Panels
            _uiFrame.HidePanel(ScreenIds.TileEditingPanel);
            
            //show the necessary Panels
            if(!_uiFrame.IsPanelOpen(ScreenIds.MenuPanel))
                _uiFrame.ShowPanel(ScreenIds.MenuPanel);
            if(!_uiFrame.IsPanelOpen(ScreenIds.ConstructionPannel))
                _uiFrame.ShowPanel(ScreenIds.ConstructionPannel);
            if(!_uiFrame.IsPanelOpen(ScreenIds.StatInfoPanel))
                _uiFrame.ShowPanel(ScreenIds.StatInfoPanel);
        }

        private void Manage_Tile_Editing_Display()
        {
            //hide the unnecessary Panels
            _uiFrame.HidePanel(ScreenIds.ConstructionPannel);
            //show the necessary Panels
            _uiFrame.ShowPanel(ScreenIds.TileEditingPanel);
        }
    }
}