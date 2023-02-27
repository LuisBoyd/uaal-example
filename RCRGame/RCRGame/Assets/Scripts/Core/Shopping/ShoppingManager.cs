using System;
using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.Optimisation.Patterns.Factory;
using RCRCoreLib.Core.Optimisation.Patterns.ObjectPooling;
using RCRCoreLib.Core.Shopping.Category;
using RCRCoreLib.Core.Systems;
using RCRCoreLib.Core.Systems.Unlockable;
using RCRCoreLib.Core.UI;
using RCRCoreLib.UI;
using RCRCoreLib.XPLevel;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Shopping
{
    [RequireComponent(typeof(CardUIViewPool), typeof(CardUIViewFactory))]
    public class ShoppingManager : Singelton<ShoppingManager>
    {
        private bool isOpened;

        //NEW

        private Dictionary<BuildingCategory, List<UnlockableBuilding>> BuildingShopItems
            = new Dictionary<BuildingCategory, List<UnlockableBuilding>>();

        private Dictionary<DecorationCategory, List<UnlockableStructure>> StructureShopItems
            = new Dictionary<DecorationCategory, List<UnlockableStructure>>();

        [SerializeField] 
        private int CardPreWarmUpQuantity = 3;
        
        private ComponentPool<CardView> CardViewpool;

        private List<CardView> CurrentVisableCards = new List<CardView>();

        private Dictionary<UnlockablePlaceables, Sprite> IconSprite
            = new Dictionary<UnlockablePlaceables, Sprite>();

        [SerializeField] 
        private ScrollRect CardViewSlider;

        [SerializeField] 
        private RectTransform BuildingCategoryButtonGroup;
        [SerializeField] 
        private RectTransform DecorationCategoryButtonGroup;

            //END NEW
        [SerializeField] private RectTransform CatergoryPannel;
        [SerializeField] private RectTransform HorizontalShopBar;
        
        [SerializeField] private RectTransform ConstructionTab;
        [SerializeField] private RectTransform BuildingsTab;

        [SerializeField] private Sprite currencyIcon;
        [SerializeField] private Sprite premiumCurrencyIcon;

        public Sprite CurrencyIcon
        {
            get => currencyIcon;
        }
        public Sprite PremiumcurrencyIcon
        {
            get => premiumCurrencyIcon;
        }
        
        protected override void Awake()
        {
            base.Awake();
            CardViewpool = GetComponent<ComponentPool<CardView>>();
            CardViewpool.Factory = GetComponent<Factory<CardView>>();
        }

        private void Start()
        {
            EventManager.Instance.AddListener<ShoppingTabGroupClicked>(ShoppingTabCategoryClicked);
            EventManager.Instance.AddListener<ClearCardViewUI>(ClearCardUIs);
            EventManager.Instance.AddListener<RefreshShopBuildingUI>(RefreshBuildingShopUI);
            EventManager.Instance.AddListener<RefreshShopDecorationUI>(RefreshDecorationsUI);
            CardViewpool.PreWarm(CardPreWarmUpQuantity);
            ConstructionTab.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<ShoppingTabGroupClicked>(ShoppingTabCategoryClicked);
            EventManager.Instance.RemoveListener<ClearCardViewUI>(ClearCardUIs);
            EventManager.Instance.RemoveListener<RefreshShopBuildingUI>(RefreshBuildingShopUI);
            EventManager.Instance.RemoveListener<RefreshShopDecorationUI>(RefreshDecorationsUI);
        }

        public void OnShop_Btn_clicked()
        {
            if (!isOpened)
            {
                isOpened = true;
                BuildingsTab.gameObject.SetActive(true);
                CatergoryPannel.gameObject.SetActive(true);
                HorizontalShopBar.gameObject.SetActive(true);
                ShoppingTabCategoryClicked(new ShoppingTabGroupClicked(ShoppingTabGroup.Building));
                ConstructionTab.gameObject.SetActive(false);
            }
            else
            {
                ConstructionTab.gameObject.SetActive(true);
                HorizontalShopBar.gameObject.SetActive(false);
                BuildingsTab.gameObject.SetActive(false);
                CatergoryPannel.gameObject.SetActive(false);
                EventManager.Instance.QueueEvent(new ClearCardViewUI());
                isOpened = false;
            }
        }

        /// <summary>
        /// Load All Building And structures for Shopping Manager
        /// </summary>
        public void Load()
        {
            foreach (BuildingCategory category in Enum.GetValues(typeof(BuildingCategory)))
                BuildingShopItems.Add(category, new List<UnlockableBuilding>());
            foreach (DecorationCategory category in Enum.GetValues(typeof(DecorationCategory)))
                StructureShopItems.Add(category, new List<UnlockableStructure>());
            
            var buildings = UnlockSystem.GetUnlockableBuildings();
            foreach (UnlockableBuilding unlockableBuilding in buildings)
            {
                BuildingShopItems[unlockableBuilding.category].Add(unlockableBuilding);
                Sprite loadedSprite = Resources.Load<Sprite>(unlockableBuilding.SpriteIconPath); //TODO Later for More Level's Change Icon
                if (loadedSprite != null)
                    IconSprite.Add(unlockableBuilding, loadedSprite);
            }

            var structures = UnlockSystem.GetUnlockableStructures();
            foreach (UnlockableStructure unlockableStructure in structures)
            {
                StructureShopItems[unlockableStructure.Category].Add(unlockableStructure);
                Sprite loadedSprite = Resources.Load<Sprite>(unlockableStructure.SpriteIconPath);
                if (loadedSprite != null)
                    IconSprite.Add(unlockableStructure, loadedSprite);
            }
        }

        private void ShoppingTabCategoryClicked(ShoppingTabGroupClicked evnt)
        {
            switch (evnt.group)
            {
                case ShoppingTabGroup.Building:
                    BuildingCategoryButtonGroup.gameObject.SetActive(true);
                    DecorationCategoryButtonGroup.gameObject.SetActive(false);
                    break;
                case ShoppingTabGroup.Decoration:
                    BuildingCategoryButtonGroup.gameObject.SetActive(false);
                    DecorationCategoryButtonGroup.gameObject.SetActive(true);
                    break;
            }
        }

        private void RefreshBuildingShopUI(RefreshShopBuildingUI evnt)
        {
            CardViewpool.Return(CurrentVisableCards);
            CurrentVisableCards.Clear();
            RefreshCardUI();
            foreach (UnlockableBuilding unlockableBuilding in BuildingShopItems[evnt.category])
            {
                CardView cardView = CardViewpool.Request();
                if(cardView == null)
                    continue;
                CurrentVisableCards.Add(cardView);
                Sprite currencyIcon = null;
                switch (unlockableBuilding.CurrentLevelInfo.costToBuildType)
                {
                    case CurrencyType.Premium:
                        currencyIcon = PremiumcurrencyIcon;
                        break;
                    case CurrencyType.Coins:
                        currencyIcon = CurrencyIcon;
                        break;
                }
                cardView.Initialize(
                    true,
                    IconSprite[unlockableBuilding],
                    currencyIcon,
                    unlockableBuilding.CurrentLevelInfo.CostToBuild,
                    unlockableBuilding.name,
                    unlockableBuilding.CurrentLevel,
                    0, //UnlockSystem.OwnedCardData.CardIDownedAmount[unlockableBuilding.ID]
                    unlockableBuilding.CurrentLevelInfo.CardsToNextLevel
                );
            }
        }
        private void RefreshDecorationsUI(RefreshShopDecorationUI evnt)
        {
            CardViewpool.Return(CurrentVisableCards);
            CurrentVisableCards.Clear();
            RefreshCardUI();
            foreach (UnlockableStructure unlockableStructure in StructureShopItems[evnt.category])
            {
                CardView cardView = CardViewpool.Request();
                if(cardView == null)
                    continue;
                CurrentVisableCards.Add(cardView);
                Sprite currencyIcon = null;
                switch (unlockableStructure.costToBuildType)
                {
                    case CurrencyType.Premium:
                        currencyIcon = PremiumcurrencyIcon;
                        break;
                    case CurrencyType.Coins:
                        currencyIcon = CurrencyIcon;
                        break;
                }
                cardView.Initialize(
                    false,
                    IconSprite[unlockableStructure],
                    currencyIcon,
                    unlockableStructure.CostToBuild,
                    unlockableStructure.name
                );
            }
        }
        private void RefreshCardUI() => CardViewSlider.gameObject.SetActive(true);
        private void ClearCardUIs(ClearCardViewUI evnt)
        {
            CardViewpool.Return(CurrentVisableCards);
            CurrentVisableCards.Clear();
            CardViewSlider.gameObject.SetActive(false);
        }


        private bool Dragging;
        public void OnBeginDrag() => Dragging = true;
        public void OnEndDrag() => Dragging = false;

        public void OnPointerClick()
        {
            if (!Dragging)
            {
                OnShop_Btn_clicked();
            }
        }

        private void OnLevelChanged(LevelChangedGameEvent evnt)
        {
            throw new NotImplementedException();
        }
    }
}