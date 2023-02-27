using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.Shopping;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.UI
{
    public class CardView : MonoBehaviour
    {
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

        //TODO Give it a way so when we drag on this CardView we Instantiate the building Ghost Object for placing.
        public void Initialize(bool showLevelProgression,
            Sprite buildingIcon, Sprite currencyIcon,
            int cost, string tittle, int LVL = 0, int cardsOwned = 0,
            int cardsToLevel = 0)
        {
            if (!showLevelProgression)
            {
                LVLDisplay.enabled = false;
                LVLProgress.enabled = false;
                CardsLeftTXT.enabled = false;
            }
            else
            {
                LVLDisplay.text = $"LVL{LVL.ToString()}";
                CardsLeftTXT.text = $"{cardsOwned}/{cardsToLevel}";
                LVLProgress.value = (cardsOwned - 0f) / (cardsToLevel - 0f);
            }

            BuildingIcon.sprite = buildingIcon;
            CurrencyIcon.sprite = currencyIcon;
            TittleDisplay.text = tittle;
            CurrencyDisplay.text = cost.ToString(); //TODO maybe try and format the Cost like with comma e.g 12000 -> 12,000
        }


    }
}