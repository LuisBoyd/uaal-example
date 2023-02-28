using System;
using System.Collections.Generic;
using RCRCoreLib.Core.AI;
using RCRCoreLib.Core.Building;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.SaveSystem;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Core.Systems.Unlockable;
using RCRCoreLib.Currency;
using RCRCoreLib.XPLevel;
using UnityEngine;

namespace RCRCoreLib.Core.Systems
{
    public class GameManager : Singelton<GameManager>
    {
        public GameObject canvas;

        public SaveData saveData;
        public UnlockData unlockData;
        
        [SerializeField] private string shopItemsPath = "shop";

        protected override void Awake()
        {
            base.Awake();
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
            Cursor.lockState = CursorLockMode.Confined; //Make's sense for a mobile game.
            saveData = SaveSystem.SaveSystem.Load();
            if(!UnlockSystem.LoadUnlocks())
                Debug.LogError($"Failed To Load Unlock Tree");
            LoadGame();
        }

        private void LoadGame()
        {
            ShoppingManager.Instance.Load();
            //LoadDelayedPlaceableObjects();
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