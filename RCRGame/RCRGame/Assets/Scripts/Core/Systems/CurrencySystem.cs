using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.Currency;
using UnityEngine;
using TMPro;

namespace RCRCoreLib.Core.Systems
{
    public class CurrencySystem : Singelton<CurrencySystem>, ISystem
    {
        private Dictionary<CurrencyType, int> CurrencyAmounts;

        [SerializeField] private List<GameObject> texts;

        private Dictionary<CurrencyType, TextMeshProUGUI> CurrencyTexts;

        protected override void Awake()
        {
            base.Awake();
            CurrencyAmounts = new Dictionary<CurrencyType, int>();
            CurrencyTexts = new Dictionary<CurrencyType, TextMeshProUGUI>();
            for (int i = 0; i < texts.Count; i++)
            {
                CurrencyAmounts.Add((CurrencyType)i,0);
                CurrencyTexts.Add((CurrencyType)i,texts[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>()); //Get the first child of the Texts game-object and get the 
                //"TextMeshProUGUI" from that child.
                
            }
        }

        private void Start()
        {
            GameManager.Instance.RegisterSystem(SystemType.CurrencySystem, this);
            EventManager.Instance.AddListener<CurrencyChangedGameEvent>(OnCurrencyChanged);
            EventManager.Instance.AddListener<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);
        }

        private void OnCurrencyChanged(CurrencyChangedGameEvent evnt)
        {
            CurrencyAmounts[evnt.currencyType] += evnt.amount;
            CurrencyTexts[evnt.currencyType].text = CurrencyAmounts[evnt.currencyType].ToString();
        }

        private void OnNotEnoughCurrency(NotEnoughCurrencyGameEvent evnt)
        {
            Debug.Log($"Not Enough Currency {evnt.amount}, {evnt.currencyType}");
        }

        public void EnableSystem()
        {
            throw new NotImplementedException();
        }

        public void DisableSystem()
        {
            throw new NotImplementedException();
        }
    }
}