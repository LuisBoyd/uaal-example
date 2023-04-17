using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Logging;


namespace UI
{
    public class LoginForm : MonoBehaviour, IInfoDisplayer
    {
        private TMP_InputField _usernameInput;
        private TMP_InputField _passwordInput;
        public TMP_Text _responseLabel;
        [HideInInspector]
        public Button _submitButton;

        [Header("Listening to")] [SerializeField]
        private InfoDisplayEventChannelSO _infoDisplayEvent;

        //private InternalSetting _setting;
        // private RuntimeLogger _logger;
        public IList<long> CodesToIgnore { get; private set; } = new List<long>()
        {

        };

        public InfoDisplayEventChannelSO Listener
        {
            get => _infoDisplayEvent;
        }

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

        private void OnEnable()
        {
            Listener.onEventRaised += DisplayInformation;
        }
        private void OnDisable()
        {
            Listener.onEventRaised -= DisplayInformation;
        }
        
        // [Inject]
        // private void InjectSettings(InternalSetting setting)
        // {
        //     _setting = setting;
        //     //_logger = logger;
        // }
        public void DisplayInformation(long code, string message, Color messageColor)
        {
            if(CodesToIgnore.Contains(code))
                return;
            _responseLabel.color = messageColor;
            _responseLabel.text = message;
        }
    }
}