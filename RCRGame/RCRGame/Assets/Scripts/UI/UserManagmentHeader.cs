using System;
using System.Collections.Generic;
using Core3.MonoBehaviors;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using VContainer;

namespace UI
{
    public class UserManagmentHeader : BaseMonoBehavior
    {
        [Serializable]
        public class Currency
        {
            [SerializeField] private Image currency_icon;
            [SerializeField] private TMP_Text currency_display; //for now rounded to 3 decimal places.
            [Header("Listening To")]
            [SerializeField] private IntEventChannelSO _onAlterntiveCurrencyChanged;
            public IntEventChannelSO onAlterntiveCurrencyChanged
            {
                get => _onAlterntiveCurrencyChanged;
            }
            public void OnCurrencyChanged(int value)
            {
                if (value >= 1000)
                {
                    currency_display.text = $"<color=green>{MathHelper.ToDecimalPlace(value, 3)}</color><color=red>{MathHelper.DeciamlCatergoryShortHand(value)}</color>";
                    return;
                }
                currency_display.text = $"<color=green>{value.ToString()}</color>";
            }
        }
        
        [SerializeField] private Image UserPfpBorder;
        [SerializeField] private Image UserPfp;
        [SerializeField] private Currency freemium_currency;
        [SerializeField] private List<Currency> alternativeCurrencies;
        
        [Header("Listening To")]
        [SerializeField] private EventRelay _userPfpBorderChanged;
        [SerializeField] private EventRelay _userPfpChanged;

        private void OnEnable()
        {
            //Register free currency event
            freemium_currency.onAlterntiveCurrencyChanged.onEventRaised += freemium_currency.OnCurrencyChanged;
            //Register all alternate currency events
            alternativeCurrencies.ForEach(currency => currency.onAlterntiveCurrencyChanged.onEventRaised += currency.OnCurrencyChanged);
        }

        private void OnDisable()
        {
            //Deregister free currency event
            freemium_currency.onAlterntiveCurrencyChanged.onEventRaised -= freemium_currency.OnCurrencyChanged;
            //Deregister all alternate currency events
            alternativeCurrencies.ForEach(currency => currency.onAlterntiveCurrencyChanged.onEventRaised -= currency.OnCurrencyChanged);
        }
    }
}