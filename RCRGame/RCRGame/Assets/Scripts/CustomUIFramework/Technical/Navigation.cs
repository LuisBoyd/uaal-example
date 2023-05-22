using System;
using CustomUIFramework.Event;
using UnityEngine;

namespace CustomUIFramework.Technical
{
    public class Navigation : MonoBehaviour
    {
        [SerializeField] 
        private ViewConfig _viewToTransitionTo;
        private NavigationEventChannelSO _navigationEventChannelSoBroadCaster;
        private void Start()
        {
            _navigationEventChannelSoBroadCaster = NavigationManager.Instance.NavigationChannel;
        }
    }
}