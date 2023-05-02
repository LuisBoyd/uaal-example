using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIArchitecture
{
    [Serializable]
    public class GameHUDButtonEntry
    {
        [Title("GameHud Button Entry Configuration", TitleAlignment = TitleAlignments.Centered)]
        [Required][SerializeField] private Sprite sprite;
        [SerializeField] private string buttonText = "";
        [SerializeField] private string targetScreen = "";
        [SerializeField] private Vector2 buttonSize = new Vector2(200, 200);

        public string ButtonText
        {
            get => buttonText;
        }

        public Sprite Sprite
        {
            get => sprite;
        }

        public Vector2 ButtonSize
        {
            get => buttonSize;
        }

        public string TargetScreen
        {
            get => targetScreen;
        }

    }
}