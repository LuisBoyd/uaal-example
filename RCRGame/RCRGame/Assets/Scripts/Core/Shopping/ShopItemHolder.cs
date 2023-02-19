using RCRCoreLib.Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Shopping
{
    public class ShopItemHolder: MonoBehaviour
    {
        private ShopItem item;

        [SerializeField] private TextMeshProUGUI tittleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image currencyIcon;
        [SerializeField] private TextMeshProUGUI priceText;

        public void Initialize(ShopItem item)
        {
            this.item = item;

            iconImage.sprite = this.item.sprite;
            tittleText.text = this.item.name;
            descriptionText.text = this.item.description;
            currencyIcon.sprite = ShoppingManager.Instance.CurrencySprites[item.currencyType];
            priceText.text = this.item.price.ToString();

            if (item.level >= LevelSystem.Instance.Level)
            {
                unlockItem();
            }
        }

        public void unlockItem()
        {
            iconImage.gameObject.AddComponent<ShopItemDrag>().Initialize(item);
            iconImage.transform.gameObject.SetActive(true);
        }
    }
}