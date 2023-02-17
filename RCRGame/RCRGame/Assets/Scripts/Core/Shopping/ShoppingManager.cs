using System;
using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.XPLevel;
using UnityEngine;

namespace RCRCoreLib.Core.Shopping
{
    public class ShoppingManager : Singelton<ShoppingManager>
    {
        public Dictionary<CurrencyType, Sprite> CurrencySprites = new Dictionary<CurrencyType, Sprite>();

        [SerializeField] private List<Sprite> sprites;

        private RectTransform Rt;
        private RectTransform Prt; //Parent's rect transform.

        private bool isOpened;

        [SerializeField] private GameObject ItemPrefab;
        private Dictionary<ObjectType, List<ShopItem>> ShopItems = new Dictionary<ObjectType, List<ShopItem>>();

        [SerializeField] public TabGroup shoptabs;

        protected override void Awake()
        {
            base.Awake();
            Rt = GetComponent<RectTransform>();
            Prt = transform.parent.GetComponent<RectTransform>();
            
        }

        private void Start()
        {
            CurrencySprites.Add(CurrencyType.Coins, sprites[0]);
            CurrencySprites.Add(CurrencyType.Dollars, sprites[1]);
            EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);
            load();
            Initialize();
            gameObject.SetActive(false);
        }

        public void OnShop_Btn_clicked()
        {
            float time = .2f;
            if (!isOpened)
            {
                LeanTween.moveX(Prt, Prt.anchoredPosition.x + Rt.sizeDelta.x, time);
                isOpened = true;
                gameObject.SetActive(true);
            }
            else
            {
                LeanTween.moveX(Prt, Prt.anchoredPosition.x - Rt.sizeDelta.x, time).setOnComplete(
                    () =>
                    {
                        gameObject.SetActive(false);
                        isOpened = false;
                    });
            }
        }

        private void load()
        {
            ShopItem[] items = Resources.LoadAll<ShopItem>("Shop");
            
            ShopItems.Add(ObjectType.Decorations, new List<ShopItem>());
            ShopItems.Add(ObjectType.Boats, new List<ShopItem>());
            ShopItems.Add(ObjectType.ProductionBuildings, new List<ShopItem>());

            foreach (var item in items)
            {
                ShopItems[item.objectType].Add(item);
            }
        }

        private void Initialize()
        {
            for (int i = 0; i < ShopItems.Keys.Count; i++)
            {
                foreach (var item in ShopItems[(ObjectType)i])
                {
                    GameObject itemObject = Instantiate(ItemPrefab, shoptabs.objectstoSwap[i].transform);
                    itemObject.GetComponent<ShopItemHolder>().Initialize(item);
                }
            }
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
            for (int i = 0; i < ShopItems.Keys.Count; i++)
            {
                ObjectType key = ShopItems.Keys.ToArray()[i];
                for (int j = 0; j < ShopItems[key].Count; j++)
                {
                    ShopItem item = ShopItems[key][j];

                    if (item.level == evnt.newLevel)
                    {
                        shoptabs.transform.GetChild(i).GetChild(j).GetComponent<ShopItemHolder>().unlockItem();
                    }
                }
            }
        }
    }
}