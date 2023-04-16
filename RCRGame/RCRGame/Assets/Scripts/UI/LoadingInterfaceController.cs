using System;
using Core.Services;
using Core3.MonoBehaviors;
using DefaultNamespace.Events;
using UnityEngine;

namespace UI
{
    public class LoadingInterfaceController : BaseMonoBehavior
    {
        [SerializeField] private GameObject _loadingInterface = default;

        [Header("Listening on")] [SerializeField]
        private BoolEventChannelSO _toggleLoadingScreen;
        private void OnEnable()
        {
            _toggleLoadingScreen.onEventRaised += ToggleLoadingScreen;
        }

        private void OnDisable()
        {
            _toggleLoadingScreen.onEventRaised -= ToggleLoadingScreen;
        }

        private void ToggleLoadingScreen(bool state)
        {
            _loadingInterface.SetActive(state);
        }
    }
}