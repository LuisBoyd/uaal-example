using System;
using Core3.MonoBehaviors;
using DefaultNamespace.Core.models;
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
        [SerializeField] private Button BuySellBtn;
        [SerializeField] private TMP_Text BuySellTMP;
        [SerializeField] private Button VisitBtn;
        [SerializeField] private Image BuySellImg;
        [SerializeField] private Image VistImg;

        public UnityAction<int> SellOwnedMarina;
        public UnityAction<int> BuyMarina;
        public UnityAction<int> VisitOwnedMarina;

        //Model
        private Mariana _mariana;
        private int _cellIndex;
        private UserMariana _UserMarianaInfo;

        private bool HasOwner
        {
            get => _UserMarianaInfo != null;
        }

        private void Start()
        {
            //Assign all the public NonAction fields in the inspector.
        }

        public void ConfigureCell(Mariana MarinaInfo, int cellIndex)
        {
            _cellIndex = cellIndex;
            _mariana = MarinaInfo;
            ApplyVisuals();
        }

        public void ConfigureCell(Mariana marianaInfo, UserMariana userMarianaInfo, int index)
        {
            _cellIndex = index;
            _mariana = marianaInfo;
            _UserMarianaInfo = userMarianaInfo;
            ApplyVisuals();
        }

        private void ApplyVisuals()
        {
            BuySellImg.color = HasOwner ? Color.red : Color.green; //For time being while setting up TODO replace with actual images
            VistImg.color = Color.blue;
            RibbonBackgroundImg.color = Color.yellow;
            MarinaNameTMP.text = _mariana.name;
            BuySellTMP.text = HasOwner ? _mariana.basesellcost.ToString() : _mariana.buycost.ToString();
        }
    }
}