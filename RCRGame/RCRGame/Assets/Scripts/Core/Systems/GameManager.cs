using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Currency;
using RCRCoreLib.XPLevel;
using UnityEngine;

namespace RCRCoreLib.Core.Systems
{
    public class GameManager : Singelton<GameManager>
    {
        public GameObject canvas;

        protected override void Awake()
        {
            base.Awake();
            ShopItemDrag.canvas = canvas.GetComponent<Canvas>();
        }

        public void GetXp(int amount)
        {
            XPAddedGameEvent info = new XPAddedGameEvent(amount);
            EventManager.Instance.QueueEvent(info);
        }

        public void GetCoins(int amount)
        {
            CurrencyChangedGameEvent info = new CurrencyChangedGameEvent(amount, CurrencyType.Coins);
            EventManager.Instance.QueueEvent(info);
        }
    }
}