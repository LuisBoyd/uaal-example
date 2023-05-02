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
        [Required] [SerializeField] private StringEventChannelSO NavigationEventChannel;
        [SerializeField] private float MainCanvasDistance = 5f;
        [SerializeField] private List<TransitionEntry> _transitionEntries = new List<TransitionEntry>();

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

        private void OnNavigateToWindow(string windowID)
        {
            _uiFrame.CloseCurrentWindow();
            _uiFrame.HideAllPanels();
            TransitionEntry entry = _transitionEntries.FirstOrDefault(entry => entry.TargetWindow.Equals(windowID));
            if (entry == null)
                throw new NullReferenceException();
            _uiFrame.OpenWindow(entry.TargetWindow);
            entry.PannelsToKeep.ForEach(panel => _uiFrame.ShowPanel(panel));
        }
    }
}