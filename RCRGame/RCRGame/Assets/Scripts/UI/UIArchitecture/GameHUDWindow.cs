using System;
using DefaultNamespace.Events;
using deVoid.UIFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIArchitecture
{
    public class GameHUDWindow : AWindowController
    {
        [Title("GameHUDWindow Configuration", TitleAlignment = TitleAlignments.Centered)] 
        [Required] [SerializeField] private EventRelay GameHudWindowBeganEvent;

        private void Start()
        {
            GameHudWindowBeganEvent.RaiseEvent();
        }
    }
}