using System;
using System.Collections.Generic;
using Core3.MonoBehaviors;
using UnityEngine;

namespace CustomUIFramework.Technical
{
    public class UIManager : Singelton<UIManager>
    {
        [SerializeField] 
        private ViewConfig firstView;

        [SerializeField]
        private List<ViewConfig> views;

        [SerializeField] 
        private SliceManager _sliceManager;
        [SerializeField] 
        private NavigationManager _navigationManager;

        protected override void Awake()
        {
            base.Awake();
            InstantiateViews();
        }

        private void InstantiateViews()
        {
            List<ViewConfig> created_views = new List<ViewConfig>();
            foreach (ViewConfig viewConfig in views)
            {
                _sliceManager.RegisterView(viewConfig);
            }
        }
    }
}