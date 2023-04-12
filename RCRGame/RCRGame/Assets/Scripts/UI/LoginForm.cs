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
        private TMP_Text _responseLabel;
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

        public void DisplayMessage(string message, LogType loglevel)
        {
            _responseLabel.text = message;
            if (_setting == null)
            {
                _responseLabel.color = Color.white;
                return;
            }
            switch (loglevel)
            {
                case LogType.Assert:
                    _responseLabel.color = _setting.DebugAssertColor;
                    break;
                case LogType.Error:
                    _responseLabel.color = _setting.DebugErrorColor;
                    break;
                case LogType.Exception:
                    _responseLabel.color = _setting.DebugExceptionColor;
                    break;
                case LogType.Log:
                    _responseLabel.color = _setting.DebugLogColor;
                    break;
                case LogType.Warning:
                    _responseLabel.color = _setting.DebugWarningColor;
                    break;
            }
        }

        private void OnEnable()
        {
            _logger.OnRuntimeLog += DisplayMessage;
        }

        private void OnDisable()
        {
            _logger.OnRuntimeLog -= DisplayMessage;
        }
    }
}