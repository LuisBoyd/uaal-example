using System;
using RCRCoreLib.Core.Building;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.SaveSystem;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Currency;
using RCRCoreLib.XPLevel;
using UnityEngine;

namespace RCRCoreLib.Core.Systems
{
    public class GameManager : Singelton<GameManager>
    {
        public GameObject canvas;

        public SaveData saveData;
        [SerializeField] private string shopItemsPath = "shop";

        protected override void Awake()
        {
            base.Awake();
            ShopItemDrag.canvas = canvas.GetComponent<Canvas>();
            SaveSystem.SaveSystem.Initialize();
        }

        public void GetXp(int amount)
        {
            XPAddedGameEvent info = new XPAddedGameEvent(amount);
            EventManager.Instance.QueueEvent(info);
        }

        public void GetCoins(int amount)
        {
            CurrencyChangedGameEvent info = new CurrencyChangedGameEvent(amount, CurrencyType.Coins);
            EventManager.Instance.QueueEvent(info);
        }

        private void Start()
        {
           saveData = SaveSystem.SaveSystem.Load();
           LoadGame();
        }

        private void LoadGame()
        {
            LoadDelayedPlaceableObjects();
        }

        private void LoadDelayedPlaceableObjects()
        {
            foreach (var plObjData in saveData.delayedPlaceableObjectDatas.Values)
            {
                try
                {
                    ShopItem item = Resources.Load<ShopItem>(shopItemsPath + "/" + plObjData.assetName);
                    GameObject obj = BuildingSystem.Instance.InitializeWithObject(item.prefab, plObjData.position, true);
                    PlaceableObject plObj = obj.GetComponent<PlaceableObject>();
                    plObj.Initialize(item, plObjData);
                    plObj.Load();
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e);
                    // throw;
                }
            }
        }

        private void OnDisable()
        {
            SaveSystem.SaveSystem.Save(saveData);
        }
    }
}