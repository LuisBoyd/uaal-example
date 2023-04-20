using System;
using Core3.MonoBehaviors;
using DefaultNamespace.Core.models;
using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.RecyclableScrollRect
{
    public class MarianaCell : BaseMonoBehavior, ICell
    {
        [SerializeField]
        private Image MarinaImage;

        [SerializeField] private TMP_Text AverageEarningsTMP;
        [SerializeField] private Image RibbonBackgroundImg;
        [SerializeField] private TMP_Text MarinaNameTMP;
        [SerializeField] private Button BuyBtn;
        [SerializeField] private TMP_Text BuyTMP;
        [SerializeField] private Button SellBtn;
        [SerializeField] private TMP_Text SellTMP;
        [SerializeField] private Button VisitBtn;
        [SerializeField] private Image BuySellImg;
        [SerializeField] private Image VistImg;

        [Header("Broadcasting ON")] 
        [SerializeField] private IntEventChannelSO BuyMarinaEventChannel;
        [SerializeField] private IntEventChannelSO SellMarinaEventChannel;
        [SerializeField] private IntEventChannelSO VisitMarinaEventChannel;

        public UnityAction<int> SellOwnedMarina;
        public UnityAction<int> BuyMarina;
        public UnityAction<int> VisitOwnedMarina;

        //Model
        private Mariana _mariana;
        private int _cellIndex;
        
        private void OnEnable()
        {
            //Assign all the Actions to the button's
            BuyBtn.onClick.AddListener(OnBuyBtnClick);
            SellBtn.onClick.AddListener(OnSellBtnClick);
            VisitBtn.onClick.AddListener(OnVistBtnClick);
        }

        private void OnDisable()
        {
            //Deassign all the Actions to the button's
            BuyBtn.onClick.RemoveListener(OnBuyBtnClick);
            SellBtn.onClick.RemoveListener(OnSellBtnClick);
            VisitBtn.onClick.RemoveListener(OnVistBtnClick);
        }

        public void ConfigureCell(Mariana MarinaInfo, int cellIndex)
        {
            _cellIndex = cellIndex;
            _mariana = MarinaInfo;
            ActivateButtons();
            ApplyVisuals();
        }

        private void ActivateButtons()
        {
            if (_mariana.OwnStatus)
            {
                BuyBtn.gameObject.SetActive(false);
                SellBtn.gameObject.SetActive(true);
            }
            else
            {
                BuyBtn.gameObject.SetActive(true);
                SellBtn.gameObject.SetActive(false);
            }
        }
        
        private void ApplyVisuals()
        {
            BuySellImg.color = _mariana.OwnStatus ? Color.red : Color.green; //For time being while setting up TODO replace with actual images
            VistImg.color = Color.blue;
            RibbonBackgroundImg.color = Color.yellow;
            MarinaNameTMP.text = _mariana.Name;
            //BuySellTMP.text = _mariana.OwnStatus ? _mariana.BaseSellCost.ToString() : _mariana.BuyCost.ToString();
        }

        #region Event-Stuff

        private void OnBuyBtnClick() => BuyMarinaEventChannel.RaiseEvent(_mariana.POIid);
        private void OnSellBtnClick() => SellMarinaEventChannel.RaiseEvent(_mariana.POIid);
        private void OnVistBtnClick() => VisitMarinaEventChannel.RaiseEvent(_mariana.POIid);

        #endregion
    }
}