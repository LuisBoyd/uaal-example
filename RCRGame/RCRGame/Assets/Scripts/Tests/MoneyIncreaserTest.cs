using System;
using Core3.MonoBehaviors;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using UnityEngine;

#if UNITY_EDITOR
using VContainer;
#endif

namespace DefaultNamespace.Tests
{
    public class MoneyIncreaserTest : BaseMonoBehavior
    {
        [SerializeField] private IntEventChannelSO FreemiumMoneyIncreaseEvent;
        [SerializeField] private IntEventChannelSO PremiumMoneyIncreaseEvent;

        private float timer = 5f;
        private float timeLeft;

        private int CurrentFreeMoneyCount;
        private int PremiumMoneyCount;

#if UNITY_EDITOR
        private User _user;
        
        //Testing for the injection just for money counting
        [Inject]
        private void InjectUser(User user)
        {
            _user = user;
        }
        
        private void Update()
        {
            if (timeLeft <= 0.0f)
            {
                timeLeft = timer;
                _user.Freemium_Currency += 100;
                _user.Premium_Currency += 50;
                FreemiumMoneyIncreaseEvent.RaiseEvent(_user.Freemium_Currency);
                PremiumMoneyIncreaseEvent.RaiseEvent(_user.Premium_Currency);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
#endif
    }
}