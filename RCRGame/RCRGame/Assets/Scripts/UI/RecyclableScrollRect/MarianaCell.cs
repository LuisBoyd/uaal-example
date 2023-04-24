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
        
        #region MarinaImage
        //TODO later it can be a full on Image instead of text
        [Header("Marina Image")]
        [SerializeField]
        private Image MarinaImage;
        [SerializeField] private TMP_Text MarinaTextInitals;

        #endregion
       

        #region BuyButton
        [Header("Buy Button")]
        [SerializeField] private Button BuyBtn;
        [SerializeField] private Image BuyCurrencyTypeIcon;
        [SerializeField] private TMP_Text CostToBuyTMP;
        #endregion
        
        #region SellButton
        [Header("Sell Button")]
        [SerializeField] private Button SellBtn;
        [SerializeField] private Image SellCurrencyTypeIcon;
        [SerializeField] private TMP_Text SellCostTMP;
        #endregion

        #region Visit Button
        [Header("Visit Button")]
        [SerializeField] private Button VisitBtn;
        //TODO later add in Lock Icon Image to activate and deactivate or something to indicate the visit is locked
        #endregion

        #region Ribbon
        [Header("Marina Information")]
        [SerializeField] private TMP_Text AverageEarningsTMP;
        [SerializeField] private Image RibbonBackgroundImg;
        [SerializeField] private TMP_Text MarinaNameTMP;
        #endregion

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
                VisitBtn.gameObject.SetActive(true);
            }
            else
            {
                BuyBtn.gameObject.SetActive(true);
                SellBtn.gameObject.SetActive(false);
                VisitBtn.gameObject.SetActive(false);
            }
        }
        
        private void ApplyVisuals()
        {
            #region BuyButton
            /*TODO BuyCurrencyTypeIcon will need to be applied later on depending on what currency
             is needed to buy the Marina 
             */
            //Cost to buy text is colored yellow hard coded although can be whatever color supported by TextMeshPro
            CostToBuyTMP.text = $"<color=yellow>{_mariana.BuyCost.ToString()}</color>";
            //The Current Buy Button on Click is red again can be changed to suit needs.
            #endregion

            #region SellButton
            /*TODO SellCurrencyTypeIcon will need to be applied later on depending on what currency
             is needed to buy the Marina 
             */
            //TODO SellCost Needs to be worked out based on certain aspects like what you have done at that marina.
            #endregion

            #region VisitButton
            //TODO Visit button if it's locked as e.g someone had max 50 maps now they only have max 10 (can happen because drop in membership) then show a lock icon over the visit button.
            #endregion

            #region RibbonDetails
            //Marina Name text is colored blue hard coded although can be whatever color supported by TextMeshPro
            MarinaNameTMP.text = _mariana.Name;
            MarinaNameTMP.color =  Color.white;
            //TODO Need to work out average earnings of a marina to apply to average earning cost.
            AverageEarningsTMP.text = "??? To be decided";
            AverageEarningsTMP.color = Color.white;
            
            //TODO depending on the status of the marina e.g Not Bought yet, Bought or Locked this can be changed to some visual color queue.
            RibbonBackgroundImg.color = _mariana.OwnStatus ? Color.yellow : Color.gray; //BUG This is not changing color of Ribbon
            
            #endregion

            #region MarinaImage

            //TODO later in the future if wanted we can grab a image for the marina rather than using it's intials
            MarinaTextInitals.text = _mariana.Name.Substring(0, 2);

            #endregion
            
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