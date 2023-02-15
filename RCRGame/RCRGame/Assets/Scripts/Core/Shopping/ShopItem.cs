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
        public CurrencyType currencyType;
        public ObjectType objectType;
        public Sprite sprite;
        public GameObject prefab;
    }
}