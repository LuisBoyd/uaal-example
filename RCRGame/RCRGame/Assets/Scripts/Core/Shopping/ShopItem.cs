using System;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.UnlockableEvents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RCRCoreLib.Core.Shopping
{
    [CreateAssetMenu(fileName = "New Item", menuName = "GameObjects/Shop Item")]
    public class ShopItem : ScriptableObject
    {
        public string name = "Default";
        
        [TextArea]
        public string description = "Description";
        
        [Tooltip("The ShopItem ID References the Unlock-able Graph ID's")]
        public int ShopItemID;

        [SerializeField]
        private bool unlocked;
        public bool Unlocked
        {
            get => unlocked;
        }
        
        public int level;
        public int price;
        public int TimeToBuild_Days = 0;
        public int TimeToBuild_Hours = 0;
        public int TimeToBuild_Minutes = 0;
        public int TimeToBuild_Seconds = 0;
        public CurrencyType currencyType;
        public ObjectType objectType;
        public Sprite sprite;
        public GameObject prefab;

        public void UnlockItem()
        {
            if(unlocked)
                return;
            
            unlocked = true;
            var unlockedItem = new UnlockedItemEvent(ShopItemID);
            EventManager.Instance.QueueEvent(unlockedItem);
        }
    }
}