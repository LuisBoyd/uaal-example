using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Events.Library.Models;
using Events.Library.Unity;
using Events.Library.Utils;
using Newtonsoft.Json;
using Patterns.ObjectPooling.Model;
using RCR.BaseClasses;
using RCR.Settings.Collections;
using RCR.Settings.NewScripts.AI;
using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.TaskSystem;
using RCR.Settings.SuperNewScripts.DontDestroyOnLoad;
using RCR.Settings.SuperNewScripts.SaveSystem.FileLoaders;
using RCR.Settings.SuperNewScripts.SaveSystem.FileSavers;
using RCR.Settings.SuperNewScripts.SaveSystem.Interfaces;
using RCR.Settings.SuperNewScripts.ScriptableObjects;
using RCR.Settings.SuperNewScripts.World.Interfaces;
using RCR.Systems.ProgressSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace RCR.Settings.SuperNewScripts
{
    [RequireComponent(typeof(Grid))]
    public class Master_World : Singelton<Master_World>, IRuntimeSaveSystem<AdjacencyMatrix<ChunkBlock>>
    , IObservableTileMap
    {
        #region Save/Load properties
        #region Tiles/Chunks
        /// <summary>
        /// Chunk Block Matrix will let me store the ChunkBlock's in a way that can
        /// I can connect them together with Edge's and Lookup adjacent Chunks
        /// </summary>
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

        // [SerializeField] 
        // private AssetReferenceT<RuleTile> Tile;

        [SerializeField] 
        private AssetReferenceT<GameObject> DefaultChunk; //For Testing TODO

        [SerializeField] 
        private TileBaselookUp TileDict;

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
        private IRuntimeSaveSystem<AdjacencyMatrix<ChunkBlock>> _runtimeSaveSystemImplementation;
#endif
        #endregion

        #region UnityFunctions
        protected override void Awake()
        {
            base.Awake();

            // FileLoader = new ChunkLoader();
            // FileSaver = new ChunkSaver();
            //Declare the Event System so that is up and running at the start on the Loaded Scene
            EventBus = new UnityEventBus(new TokenUtils());
//
            var s = GameConstants.DefaultSerializerSettings;
            JsonSerializer serializer = JsonSerializer.Create(s);

            var text = File.ReadAllText(Path.Combine(Application.dataPath, "2D/DefaultChunk_AstonMarina.json"));

            var chunk = JsonConvert.DeserializeObject<ChunkBlock>(text);
            
            ///
            
            statCalculator = StatisticsCalculator.GetInstance();
            TileDataBase = SuperNewScripts.TileDataBase.GetInstance();
            StructureDataBase = SuperNewScripts.StructureDataBase.GetInstance();
            AdaptivePerformanceSystem =
                SuperNewScripts.AdaptivePerformanceSystem.GetInstance();
            AdvertismentSystem = SuperNewScripts.AdvertismentSystem.GetInstance();
            LoggingSystem = SuperNewScripts.LoggingSystem.GetInstance();
            
            
            WorldGrid = GetComponent<Grid>();
            
            TilemapRenderer tilemapRenderer = new GameObject("World Tilemap").AddComponent<TilemapRenderer>();
            WorldTilemap = tilemapRenderer.gameObject.GetComponent<Tilemap>();
            tilemapRenderer.mode = TilemapRenderer.Mode.Chunk;
            WorldTilemap.origin = Vector3Int.zero;
            WorldTilemap.size = new Vector3Int(GameConstants.ChunkSize, GameConstants.ChunkSize
                , 1);
            WorldTilemap.ResizeBounds();
            WorldTilemap.gameObject.transform.SetParent(this.transform);
            WorldTilemap.gameObject.transform.localPosition = Vector3.zero;
            InitialData.SetLocationID("Aston.json");


        }

        private async UniTaskVoid Start()
        {
            var LoadOperation = await Load();
            if(LoadOperation)
                return;
            //NEED TO INITIALISE A NEW INSTANCE

            Value = new AdjacencyMatrix<ChunkBlock>(GameConstants.WorldMaxSize,
                GameConstants.WorldMaxSize, true);
            
            //Init ChunkOrigins
            for (int x = 0; x < GameConstants.WorldMaxSize; x++)
            for (int y = 0; y < GameConstants.WorldMaxSize; y++)
            {
                var Node = Value.GetNode(x, y);
                Node.SetOrigin(new Vector2Int(
                    x * GameConstants.ChunkSize, y * GameConstants.ChunkSize));
                Node.SetActive(false);
            }

            //Check If Any Overlap
            for (int x = 0; x < Value.GetLength(0); x++)
            for (int y = 0; y < Value.GetLength(1); y++)
                if (!Value.CheckOtherNodes(x, y,
                        (C1, C2) => ((C1.Origin.x < C2.Origin.x + GameConstants.ChunkSize) &&
                                     (C1.Origin.x + GameConstants.ChunkSize > C2.Origin.x) &&
                                     (C1.Origin.y < C2.Origin.y + GameConstants.ChunkSize) &&
                                     (C1.Origin.y + GameConstants.ChunkSize > C2.Origin.y))))
                {
                    //Something Overlaps
                    Debug.LogError("Chunks Overlap");
                    return;
                }

            var TilemapOperation = await LoadDefaultChunk();
            if(!TilemapOperation.Succeeded)
                return;
            

        }

        #endregion


        private Grid WorldGrid;
        private Tilemap WorldTilemap;
        private PolygonCollider2D WorldBounds;
        //Could use renderer in future but not needed now TODO
        public IFileLoader<AdjacencyMatrix<ChunkBlock>> FileLoader { get; private set; }
        public IFileSaver<AdjacencyMatrix<ChunkBlock>> FileSaver { get; private set; }
        public AdjacencyMatrix<ChunkBlock> Value { get; private set; }

        public async UniTaskVoid Save()
        {
            FileSaver.WriteToFile(InitialData.LocationID, Value).Forget();
        }

        public async UniTask<bool> Load( AdjacencyMatrix<ChunkBlock> overWriteObj)
        {
            //Probably don't use
            var operation = await FileLoader.ReadFromFile(InitialData.LocationID);
            if (!operation.Succeeded)
                return false;
            overWriteObj = operation.Value;
            return true;
        }

        public async UniTask<bool> Load()
        {
            var operation = await FileLoader.ReadFromFile(InitialData.LocationID);
            if (!operation.Succeeded)
                return false;
            Value = operation.Value;
            return true;
        }

        public Tilemap Tilemap
        {
            get => WorldTilemap;
        }
        public void SetTile(Vector3Int position, TileBase tile)
        {
            Tilemap.SetTile(position, tile);
            EventBus.Publish_Coroutine(new WorldEvent.WorldTilemapChanged(
                    new BoundsInt(position, Vector3Int.one), new TileBase[] { tile }),
                EventArgs.Empty); //Change the coroutine for Unitask TODO
        }

        public void SetTiles(Vector3Int[] positions, TileBase[] tiles)
        {
            Tilemap.SetTiles(positions, tiles);
            EventBus.Publish_Coroutine(new WorldEvent.WorldTilemapChanged(
                    positions, tiles),
                EventArgs.Empty); //Change the coroutine for Unitask TODO
        }

        public void SetTilesBlock(BoundsInt position, TileBase[] tiles)
        {
            Tilemap.SetTilesBlock(position, tiles);
            EventBus.Publish_Coroutine(new WorldEvent.WorldTilemapChanged(
                    position, tiles),
                EventArgs.Empty); //Change the coroutine for Unitask TODO
        }

        public void MassSetTilesBlock(List<KeyValuePair<BoundsInt, TileBase[]>> kvp)
        {
            foreach (var KVP in kvp)
            {
                Tilemap.SetTilesBlock(KVP.Key, KVP.Value);
            }
            Debug.Log("Mass Setted");
        }

        public async UniTask<OperationResult<TileBase[]>> LoadDefaultChunk()
        {
            var loadOperation = await AddresablesSystem.Instance.LoadAssetAsync<GameObject>(
                DefaultChunk);
            if (!loadOperation.Succeeded)
                return new OperationResult<TileBase[]>(false, default, null);
            var tilemap = loadOperation.Value.GetComponent<Tilemap>();
            if (tilemap.size.x <= GameConstants.ChunkSize && tilemap.size.y <= GameConstants.ChunkSize
                                                          && tilemap.size.x > 0 && tilemap.size.y > 0)
            {
                TileBase[] tiles = tilemap.GetTilesBlock(tilemap.cellBounds);
                return new OperationResult<TileBase[]>(true, default, tiles);
            }
            return new OperationResult<TileBase[]>(false, default, null);
        }

        public async UniTaskVoid SortVisualsDefault()
        {
            var ActiveNodes = Value.GetNodes().Where(x => x.Active);
            List<KeyValuePair<BoundsInt, TileBase[]>> NodeData = new List<KeyValuePair<BoundsInt, TileBase[]>>();
            foreach (var AC in ActiveNodes)
            {
                var DT = AC.Tiles;
                TileBase[] TilesToPlace = new TileBase[DT.GetLength(0) * DT.GetLength(1)];

                for (int i = 0; i < TilesToPlace.Length; i++)
                {
                    TilesToPlace[i] = await AddresablesSystem.Instance.LoadAssetAsync<TileBase>(
                        DT[i / GameConstants.ChunkSize, i % GameConstants.ChunkSize].VisualKey.ToString());
                }
                
                BoundsInt ACBoundInt = new BoundsInt(new Vector3Int(AC.Origin.x, AC.Origin.y),
                    new Vector3Int(GameConstants.ChunkSize, GameConstants.ChunkSize, 1));
                
                NodeData.Add(new KeyValuePair<BoundsInt, TileBase[]>(ACBoundInt, TilesToPlace));
            }
            
            MassSetTilesBlock(NodeData);
        }
        public async UniTaskVoid SortVisualsDefault(TileBase[] tiles)
        {
            DataTile[] dataTiles = new DataTile[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                //Check if Tile is apart of TileBaseDict
                if (TileDict.TryGetValue(tiles[i], out AssetReferenceT<TileBase>
                        referenceT))
                {
                    dataTiles[i] = DataTile.Create(referenceT.RuntimeKey.ToString());
                }
                else
                {
                    dataTiles[i] = DataTile.Create("0");
                }
                
            }

            var Node = Value.GetNode(0, 0);
            Node.SetActive(true);
            Node.SetDataTiles(dataTiles);
            SortVisualsDefault().Forget();
        }
    }
}