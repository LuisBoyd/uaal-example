using System;
using deVoid.UIFramework.Extends.Premade.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deVoid.UIFramework.Extends.Premade
{
    [RequireComponent(typeof(Button))]
    public class NavigationPanelButton : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshPro _buttonLabel = null;
        [SerializeField] 
        private Image _icon = null;

        public event Action<NavigationPanelButton> ButtonClicked;

        private NavigationPanelEntry _navigationData = null;
        private Button _button = null;

        public Button Button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                return _button;
            }
        }

        public string Target
        {
            get { return _navigationData.TargetScreen; }
        }

        public void SetData(NavigationPanelEntry target)
        {
            _navigationData = target;
            _buttonLabel.text = target.ButtonText;
            _icon.sprite = target.Sprite;
        }

        public void SetCurrentNavigationTarget(NavigationPanelButton selectedButton)
        {
            _button.interactable = selectedButton != this;
        }

        public void SetCurrentNavigationTarget(string screenID)
        {
            if (_navigationData != null)
            {
                _button.interactable = _navigationData.TargetScreen == screenID;
            }
        }

        //Assign to the button component
        public void UI_Click()
        {
            if (ButtonClicked != null)
                ButtonClicked(this);
        }
    }
}