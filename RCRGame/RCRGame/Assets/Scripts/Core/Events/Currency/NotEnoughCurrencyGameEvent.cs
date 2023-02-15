using RCRCoreLib.Core.Enums;

namespace RCRCoreLib.Currency
{
    public class NotEnoughCurrencyGameEvent : GameEvent
    {
        public int amount;
        public CurrencyType currencyType;

        public NotEnoughCurrencyGameEvent(int amount, CurrencyType currencyType)
        {
            this.amount = amount;
            this.currencyType = currencyType;
        }
    }
}