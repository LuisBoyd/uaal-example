using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoginForm : MonoBehaviour
    {

        private TMP_InputField _usernameInput;
        private TMP_InputField _passwordInput;
        private TMP_Text _responseLabel;
        [HideInInspector]
        public Button _submitButton;
        
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
    }
}