using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Events.Library.Models;
using Events.Library.Unity;
using Patterns.ObjectPooling.Model;
using RCR.BaseClasses;
using RCR.Settings.Collections;
using RCR.Settings.NewScripts.AI;
using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.TaskSystem;
using RCR.Systems.ProgressSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RCR.Settings.SuperNewScripts
{
    public class Master_World : Singelton<Master_World>
    {
        #region Save/Load properties
        #region Tiles/Chunks
        /// <summary>
        /// Chunk Block Matrix will let me store the ChunkBlock's in a way that can
        /// I can connect them together with Edge's and Lookup adjacent Chunks
        /// </summary>
        public AdjacencyMatrix<ChunkBlock> ChunkBlock_Matrix { get; private set; }
        #endregion
        #region Progress
        /*Things such as buildings progress on leaving the game and the progression they are
         currently at
        */
        public ProgressSystem ProgressSystem { get; private set; }
        #endregion

        #region Buildings/Strucutres
        /*  I will give the building sprites ID's like the tiles, this way they too can just
         *  be looked up at startTime, also storing things such as the buildings GridSpace
         *  as well as there own upgrades if I put this all into one object it should make
         *  it easier for serialization
         */ 
        public List<StructureData> StructureData { get; private set; }
        #endregion
        
        #region Statisitcs

        /// <summary>
        /// Later on to do with graph's and such
        /// </summary>
        private StatisticsCalculator statCalculator;
        #endregion
        #endregion

        #region WorldEvents
        public IUnityEventBus EventBus { get; private set; }
        public Dictionary<WorldEvent, Token> WorldEventTokens { get; private set; }
        #endregion
        #region LookUpDataSets

        public TileDataBase TileDataBase;
        public StructureDataBase StructureDataBase;
        #endregion

        #region AI
        public PathFindingSystem PathFindingSystem = NewScripts.AI.PathFindingSystem.GetInstance();
        #endregion

        #region Systems

        public AdaptivePerformanceSystem AdaptivePerformanceSystem;

        #region Addresable System

        [SerializeField] 
        private AssetLabelReference[] _LoadAssetLabelOnStart;
        
        public AddresablesSystem AddresablesSystem;
        #endregion
        public AdvertismentSystem AdvertismentSystem;
        public LoggingSystem LoggingSystem;
        #endregion

        #region Entity's
        public TaskSystem<BoatTask> boatTaskSystem { get; private set; }
        public TaskSystem<CustomerTask> CustomerTaskSystem { get; private set; }
        public ComponentPool<Boat> BoatPool { get; private set; }
        public ComponentPool<Boat> CustomerPool { get; private set; } //TODO these Derived
        //mono behaviors will have to be checked
        #endregion
        
        #region CustomEnums
        public enum ChunkSize
        {
            x2 = 2,
            x4 = 4,
            x8 = 8,
            x16 = 16,
            x32 = 32,
            x64 = 64,
            x128 = 128,
            x256 = 256,
            x512 = 512,
            x1024 = 1024,
            x2048 = 2048,
            x4096 = 4096
        }
        public enum WorldSize
        {
            x2 = 2,
            x4 = 4,
            x6 = 6,
            x8 = 8,
            x10 = 10,
            x12 = 12
        }
        #endregion
        
        #region EditorOnly
#if UNITY_EDITOR
        public ChunkSize Csize;
        public WorldSize Wsize;
#endif
        #endregion

        #region UnityFunctions
        protected override void Awake()
        {
            base.Awake();
            statCalculator = StatisticsCalculator.GetInstance();
            TileDataBase = SuperNewScripts.TileDataBase.GetInstance();
            StructureDataBase = SuperNewScripts.StructureDataBase.GetInstance();
            AdaptivePerformanceSystem =
                SuperNewScripts.AdaptivePerformanceSystem.GetInstance();
            AddresablesSystem = SuperNewScripts.AddresablesSystem.GetInstance();
            AdvertismentSystem = SuperNewScripts.AdvertismentSystem.GetInstance();
            LoggingSystem = SuperNewScripts.LoggingSystem.GetInstance();

        }

        private async UniTaskVoid Start()
        {
            AddresablesSystem.Start().Forget();
            AddresablesSystem.LoadAssetLabel(_LoadAssetLabelOnStart).Forget();
        }

        #endregion

        #region ImportantMethods

        private void InitViusals()
        {
            
        }

        #endregion
        
    }
}