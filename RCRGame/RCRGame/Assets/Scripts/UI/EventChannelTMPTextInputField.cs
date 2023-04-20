using System;
using Core3.MonoBehaviors;
using DefaultNamespace.Events;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class EventChannelTMPTextInputField : BaseMonoBehavior
    {
        [Header("Broadcasting On")]
        [SerializeField] 
        private StringEventChannelSO OnTextChanged;


        private TMP_InputField _inputField;
        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        protected void OnEnable()
        {
           _inputField.onValueChanged.AddListener(EventSendWholeText);
        }

        protected  void OnDisable()
        {
            _inputField.onValueChanged.RemoveListener(EventSendWholeText);
        }

        private void EventSendWholeText(string text)
        {
            OnTextChanged.RaiseEvent(_inputField.text);
        }
    }
}