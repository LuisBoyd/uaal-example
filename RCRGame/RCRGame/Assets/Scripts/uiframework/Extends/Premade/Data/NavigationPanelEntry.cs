using System;
using UnityEngine;

namespace deVoid.UIFramework.Extends.Premade.Data
{
    [Serializable]
    public class NavigationPanelEntry
    {
        [SerializeField] private Sprite _sprite = null;
        [SerializeField] private string _buttonText = "";
        [SerializeField] private string _targetScreen = "";
        
        public Sprite Sprite
        {
            get => _sprite;
        }

        public string ButtonText
        {
            get { return _buttonText; }
        }

        public string TargetScreen
        {
            get { return _targetScreen; }
        }
    }
}