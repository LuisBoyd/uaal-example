using System;
using RCRCoreLib.Core.Enums;
using UnityEngine;

namespace RCRCoreLib.Core.Shopping
{
    [CreateAssetMenu(fileName = "New Item", menuName = "GameObjects/Shop Item")]
    public class ShopItem : ScriptableObject
    {
        public string name = "Default";
        public string description = "Description";
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
    }
}