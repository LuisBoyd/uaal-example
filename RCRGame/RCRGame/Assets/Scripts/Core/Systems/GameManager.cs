using System;
using System.Collections.Generic;
using RCRCoreLib.Core.AI;
using RCRCoreLib.Core.Building;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.Currency;
using RCRCoreLib.Core.Events.System;
using RCRCoreLib.Core.Events.XPLevel;
using RCRCoreLib.Core.SaveSystem;
using RCRCoreLib.Core.Shopping;
using RCRCoreLib.Core.Systems.Tutorial;
using RCRCoreLib.Core.Systems.Unlockable;
using RCRCoreLib.Core.Utilities.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Systems
{
    public class GameManager : Singelton<GameManager>
    {
        public GameObject canvas;

        public SaveData saveData;
        public UnlockData unlockData;
        
        [SerializeField] private string shopItemsPath = "shop";

        private IDictionary<SystemType, ISystem> SystemDictionary;

        protected override void Awake()
        {
            base.Awake();
            SystemDictionary = new Dictionary<SystemType, ISystem>();
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
            EventManager.Instance.AddListener<SystemActivateEvent>(On_SystemActivate);
            Cursor.lockState = CursorLockMode.Confined; //Make's sense for a mobile game.
            saveData = SaveSystem.SaveSystem.Load();
            if(!UnlockSystem.LoadUnlocks())
                Debug.LogError($"Failed To Load Unlock Tree");
            LoadGame();
        }

        public void RegisterSystem(SystemType type, ISystem system) => SystemDictionary.Add(type, system);
        
        private void On_SystemActivate(SystemActivateEvent e)
        {
            ISystem instance = SystemDictionary[e.type];
            if(e.Active)
                instance.EnableSystem();
            else
            {
                instance.DisableSystem();
            }
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
            EventManager.Instance.RemoveListener<SystemActivateEvent>(On_SystemActivate);
            SaveSystem.SaveSystem.Save(saveData);
        }
    }
}