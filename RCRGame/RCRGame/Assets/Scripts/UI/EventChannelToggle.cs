using System;
using Core3.MonoBehaviors;
using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Toggle))]
    public class EventChannelToggle : BaseMonoBehavior
    {
        private Toggle _toggle;

        [Header("Broadcasting On")] 
        [SerializeField] private BoolEventChannelSO OnToggled;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OnToggled.RaiseEvent);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OnToggled.RaiseEvent);
        }
    }
}