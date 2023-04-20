using System;
using Core3.MonoBehaviors;
using DefaultNamespace.Events;
using UnityEngine;

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
        private void Update()
        {
            if (timeLeft <= 0.0f)
            {
                timeLeft = timer;
                CurrentFreeMoneyCount += 100;
                PremiumMoneyCount += 50;
                FreemiumMoneyIncreaseEvent.RaiseEvent(CurrentFreeMoneyCount);
                PremiumMoneyIncreaseEvent.RaiseEvent(PremiumMoneyCount);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
#endif
    }
}