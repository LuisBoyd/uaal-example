using System.Collections.Generic;
using Core.Optimisation.Patterns.Factory;
using Core.Optimisation.Patterns.ObjectPooling;
using DefaultNamespace.Events;
using deVoid.UIFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIArchitecture
{

    public class TilePlacementControlPanel : ControlPanel
    {
        [Title("Construction Panel Controller Configurations", TitleAlignment = TitleAlignments.Centered)]
        [Required] [SerializeField] private GameTransitionHUDButton backButton;
        [SerializeField] [Required] private BoolEventChannelSO TileSystemStateSwitchSo;

        protected override void Awake()
        {
            base.Awake();
            backButton.SetTarget(ScreenIds.GameHUDWindow);
        }

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            TileSystemStateSwitchSo.RaiseEvent(true);
        }

        protected override void WhileHiding()
        {
            base.WhileHiding();
            TileSystemStateSwitchSo.RaiseEvent(false);
        }
    }
}