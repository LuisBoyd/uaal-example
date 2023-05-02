using System;
using System.Collections.Generic;
using Core.Optimisation.Patterns.Factory;
using Core.Optimisation.Patterns.ObjectPooling;
using deVoid.UIFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIArchitecture
{
    [RequireComponent(typeof(ComponentPool<GameTransitionHUDButton>))]
    [RequireComponent(typeof(Factory<GameTransitionHUDButton>))]
    [RequireComponent(typeof(ComponentPool<GameHUDButton>))]
    [RequireComponent(typeof(Factory<GameHUDButton>))]
    public class ControlPanel : APanelController
    {
        [Title("Control Panel Configurations", TitleAlignment = TitleAlignments.Centered)]
        [Required] [SerializeField] protected Transform ParentRect = null;
        [SerializeField, ReadOnly] protected ComponentPool<GameTransitionHUDButton> _buttonTransitionComponentPool;
        [SerializeField, ReadOnly] protected ComponentPool<GameHUDButton> _buttonComponentPool;
        [SerializeField] protected int PrewarmButtonCount = 3;
        [SerializeField] private List<GameHUDButtonEntry> ConstructionOptionEntries = new List<GameHUDButtonEntry>();

        protected List<GameHUDButton> _currentButtons = new List<GameHUDButton>();

        protected override void Awake()
        {
            base.Awake();
            _buttonComponentPool = GetComponent<ComponentPool<GameHUDButton>>();
            _buttonTransitionComponentPool = GetComponent<ComponentPool<GameTransitionHUDButton>>();
            _buttonComponentPool.Factory = GetComponent<Factory<GameHUDButton>>();
            _buttonTransitionComponentPool.Factory = GetComponent<Factory<GameTransitionHUDButton>>();
            
            _buttonTransitionComponentPool.Prewarm(PrewarmButtonCount);
            _buttonComponentPool.Prewarm(PrewarmButtonCount);
        }

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            SortButtons();
        }

        protected override void WhileHiding()
        {
            base.WhileHiding();
            _currentButtons.ForEach(button =>
            {
                if (button is GameTransitionHUDButton transitionHUDButton)
                {
                    _buttonTransitionComponentPool.Return(transitionHUDButton);
                }
                else
                {
                    _buttonComponentPool.Return(button);
                }
            });
            _currentButtons.Clear();
        }

        protected virtual void SortButtons()
        {
            foreach (GameHUDButtonEntry entry in ConstructionOptionEntries)
            {
                if (entry.IsTransition)
                {
                    var transitionButton = _buttonTransitionComponentPool.Request();
                    transitionButton.transform.SetParent(ParentRect, false);
                    transitionButton.SetData(entry);
                    _currentButtons.Add(transitionButton);
                }
                else
                {
                    var HUDButton = _buttonComponentPool.Request();
                    HUDButton.transform.SetParent(ParentRect, false);
                    HUDButton.SetData(entry);
                    _currentButtons.Add(HUDButton);
                }
            }
        }
        
    }
}