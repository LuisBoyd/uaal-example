using System.Collections;
using System.Collections.Generic;
using RCR.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.uGUI
{
    public class Icon_Attraction : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _buildingTitle;
        [SerializeField] 
        private TMP_Text _costText;
        [SerializeField] 
        private Image _currency;
        [SerializeField] 
        private Image _display;

        public string BuildingTittle
        {
            set => _buildingTitle.text = value;
        }
        public int Cost
        {
            set => _costText.text = value.ToString();
        }
        public Image Currency
        {
            set => _currency = value;
        }
        public Texture2D Display
        {
            set => _display.sprite = LBUtilities.SpriteFromTexture(value);
        }
    }
}
