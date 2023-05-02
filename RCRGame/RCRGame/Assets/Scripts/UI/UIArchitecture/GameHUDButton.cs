using System;
using Core3.MonoBehaviors;
using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIArchitecture
{
    [RequireComponent(typeof(Button))]
    public class GameHUDButton : BaseMonoBehavior
    {
        [Title("Game HUD Button Configuration", TitleAlignment = TitleAlignments.Centered)] 
        [SerializeField] [Required] private TextMeshProUGUI buttonLabel = null;
        [SerializeField] [Required] private Image icon = null;
        [SerializeField] [Required] private RectTransform _rect = null;

        public EventRelay ButtonClicked;
        private Button _button = null;

        private Button Button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                return _button;
            }
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            if (ButtonClicked == null)
                ButtonClicked = ScriptableObject.CreateInstance<EventRelay>();
        }

        private void OnEnable()
        {
            Button.onClick.AddListener(UI_Click);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(UI_Click);
        }

        public virtual void SetData(GameHUDButtonEntry buttonEntry)
        {
            buttonLabel.text = buttonEntry.ButtonText;
            icon.sprite = buttonEntry.Sprite;
            _rect.sizeDelta = buttonEntry.ButtonSize;
        }

        protected virtual void UI_Click()
        {
            if (ButtonClicked != null)
            {
                ButtonClicked.RaiseEvent();
            }
        }

    }
}