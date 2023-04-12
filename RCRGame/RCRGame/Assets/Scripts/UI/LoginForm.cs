using System;
using System.Linq;
using Core3.SciptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Logging;
using VContainer;

namespace UI
{
    public class LoginForm : MonoBehaviour
    {

        private TMP_InputField _usernameInput;
        private TMP_InputField _passwordInput;
        public TMP_Text _responseLabel;
        [HideInInspector]
        public Button _submitButton;

        private InternalSetting _setting;
        private RuntimeLogger _logger;
        
        public string Username
        {
            get => _usernameInput.text;
        }
        public string Password
        {
            get => _passwordInput.text;
        }

        private void Awake()
        {
            var inputs = GetComponentsInChildren<TMP_InputField>();
            _usernameInput = inputs.FirstOrDefault(i => i.name.StartsWith("U"));
            _passwordInput = inputs.FirstOrDefault(i => i.name.StartsWith("P"));
            _submitButton = GetComponentInChildren<Button>();
            _responseLabel = GetComponentInChildren<TMP_Text>();
            _responseLabel.text = String.Empty;
        }

        [Inject]
        private void InjectSettings(InternalSetting setting, RuntimeLogger logger)
        {
            _setting = setting;
            _logger = logger;
        }
    }
}