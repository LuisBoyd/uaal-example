﻿using RCRCoreLib.Core.Enums;

namespace RCRCoreLib.Currency
{
    public class CurrencyChangedGameEvent : GameEvent
    {
        public int amount;
        public CurrencyType currencyType;

        public CurrencyChangedGameEvent(int amount, CurrencyType currencyType)
        {
            this.amount = amount;
            this.currencyType = currencyType;
        }
    }
}