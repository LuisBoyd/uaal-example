using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.Currency;
using RCRCoreLib.Core.Events.XPLevel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RCRCoreLib.Core.Systems
{
    public class LevelSystem : Singelton<LevelSystem>, ISystem
    {
        private int XPnow;
        public int Level { get; private set; }
        private int XpToNextLevel;

        [SerializeField] private GameObject levelPannel;
        [SerializeField] private GameObject LevelWindowPrefab;
        
        //TODO replace with a more concrete method of getting UGUI

        // [SerializeField]private Slider _slider;
        // [SerializeField]private TextMeshProUGUI xpText;
        [SerializeField]private TextMeshProUGUI lvlText;
        [SerializeField]private Image StarImage;

        private bool Initialized;

        private Dictionary<int, int> xpToLevelUp = new Dictionary<int, int>()
        {
            { 0, 10 },
            { 1, 30 },
            { 2, 50 }
        };
        private Dictionary<int, int[]> lvlReward = new Dictionary<int, int[]>()
        {
            {0, new int[]
            {
                10,10,5
            }},
            {1, new int[]
            {
                15,15,10
            }},
            {2, new int[]
            {
                20,20,15
            }},
            
        };

        protected override void Awake()
        {
            base.Awake();
            if (!Initialized)
                Initialise();

            xpToLevelUp.TryGetValue(Level, out XpToNextLevel);
        }

        private void Start()
        {
            GameManager.Instance.RegisterSystem(SystemType.LevelSystem, this);
            EventManager.Instance.AddListener<XPAddedGameEvent>(onXpAdded);
            EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);
            UpdateUI();
        }

        private void Initialise()
        {
            //TODO Player Level Initialisation
            // - Cloud Database
            // realistically if this fails should just not let the player into the game as server is down or something.
        }

        private void UpdateUI()
        {
            float fill = (float)XPnow / XpToNextLevel;
            // _slider.value = fill;
            // xpText.text = $"{XPnow}/{XpToNextLevel}";
        }

        private void onXpAdded(XPAddedGameEvent evnt)
        {
            XPnow += evnt.amount;
            UpdateUI();
            if (XPnow >= XpToNextLevel)
            {
                Level++;
                LevelChangedGameEvent levelChange = new LevelChangedGameEvent(Level);
                EventManager.Instance.QueueEvent(levelChange);
            }
        }

        private void OnLevelChanged(LevelChangedGameEvent evnt)
        {
            XPnow -= XpToNextLevel;
            XpToNextLevel = xpToLevelUp[evnt.newLevel];
            lvlText.text = $"{evnt.newLevel + 1}";
            UpdateUI();

            GameObject widnow = Instantiate(LevelWindowPrefab, GameManager.Instance.canvas.transform);
            
            widnow.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
            {
                Destroy(widnow);
            });

            CurrencyChangedGameEvent currencyChangedGameEvent = new CurrencyChangedGameEvent(lvlReward[evnt.newLevel][0], CurrencyType.Coins);
            EventManager.Instance.QueueEvent(currencyChangedGameEvent);
            currencyChangedGameEvent = new CurrencyChangedGameEvent(lvlReward[evnt.newLevel][0], CurrencyType.Coins);
            EventManager.Instance.QueueEvent(currencyChangedGameEvent);
        }

        public void EnableSystem()
        {
            throw new NotImplementedException();
        }

        public void DisableSystem()
        {
            throw new NotImplementedException();
        }
    }
}