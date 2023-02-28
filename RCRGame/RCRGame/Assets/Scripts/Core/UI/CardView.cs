using System;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Core.Systems.Unlockable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] 
        private Image CardBackGround;
        [SerializeField] 
        private Image BuildingIcon;
        [SerializeField] 
        private Image CurrencyIcon;
        [SerializeField] 
        private TextMeshProUGUI CurrencyDisplay;
        [SerializeField] 
        private TextMeshProUGUI TittleDisplay;
        [SerializeField] 
        private TextMeshProUGUI LVLDisplay;
        [SerializeField] 
        private Slider LVLProgress;
        [SerializeField] 
        private TextMeshProUGUI CardsLeftTXT;

        [SerializeField] 
        private Color LockedColor;
        [SerializeField] 
        private Color UnlockedColor;

        [SerializeField] 
        private Color LockedTextColor;
        [SerializeField] 
        private Color UnlockedTextColor;

        private ShopItemDrag ItemDrag;

        private void Awake()
        {
            ItemDrag = BuildingIcon.gameObject.GetComponent<ShopItemDrag>();
        }

        //TODO Give it a way so when we drag on this CardView we Instantiate the building Ghost Object for placing.
        public void Initialize(bool showLevelProgression,
            UnlockablePlaceables placeable, Sprite currencyIcon,
            int cost, string tittle, int LVL = 0, int cardsOwned = 0,
            int cardsToLevel = 0)
        {
            CardBackGround.color = UnlockedColor;
            LVLDisplay.color = UnlockedTextColor;
            LVLDisplay.enableAutoSizing = false;
            LVLProgress.gameObject.SetActive(true);
            CardsLeftTXT.enabled = true;
            CurrencyIcon.enabled = true;
            CurrencyDisplay.enabled = true;
            if (!showLevelProgression && placeable.Unlocked)
            {
                LVLDisplay.enabled = false;
                LVLProgress.gameObject.SetActive(false);
                CardsLeftTXT.enabled = false;
            }
            else if(showLevelProgression && placeable.Unlocked)
            {
                LVLDisplay.text = $"LVL{LVL.ToString()}";
                CardsLeftTXT.text = $"{cardsOwned}/{cardsToLevel}";
                LVLProgress.value = (cardsOwned - 0f) / (cardsToLevel - 0f);
            }
            else if (!placeable.Unlocked)
            {
                CardBackGround.color = LockedColor;
                LVLDisplay.color = LockedTextColor;
                LVLDisplay.enableAutoSizing = true;
                LVLDisplay.text = placeable.LockedMessage;
                CurrencyIcon.enabled = false;
                CurrencyDisplay.enabled = false;
                LVLProgress.gameObject.SetActive(false);
                CardsLeftTXT.enabled = false;
            }
            BuildingIcon.sprite = placeable.Icon;
            CurrencyIcon.sprite = currencyIcon;
            TittleDisplay.text = tittle;
            CurrencyDisplay.text = cost.ToString(); //TODO maybe try and format the Cost like with comma e.g 12000 -> 12,000
            ItemDrag.enabled = placeable.Unlocked;
        }


    }
}