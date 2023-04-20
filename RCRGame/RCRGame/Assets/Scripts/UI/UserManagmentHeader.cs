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
            [SerializeField] private TMP_Text tenthsDisplay;
            [Header("Listening To")]
            [SerializeField] private IntEventChannelSO _onAlterntiveCurrencyChanged;

            public void OnCurrencyChanged(int value)
            {
                currency_display.text = $"<color=green>{MathHelper.ToDecimalPlace(value, 3)}</color>";
                tenthsDisplay.text = $"<color=red>{MathHelper.DeciamlCatergory(value)}</color>";
            }
        }
        
        [SerializeField] private Image UserPfpBorder;
        [SerializeField] private Image UserPfp;
        [SerializeField] private Currency freemium_currency;
        [SerializeField] private List<Currency> alternativeCurrencies;
        
        [Header("Listening To")]
        [SerializeField] private EventRelay _userPfpBorderChanged;
        [SerializeField] private EventRelay _userPfpChanged;
    }
}