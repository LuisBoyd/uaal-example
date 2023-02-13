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
using QuikGraph;
using RCR.BaseClasses;
using RCR.Settings.Collections;
using RCR.Settings.Collections.Sorting;
using RCR.Settings.NewScripts.AI;
using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.Geometry;
using RCR.Settings.NewScripts.TaskSystem;
using RCR.Settings.SuperNewScripts.DataStructures;
using RCR.Settings.SuperNewScripts.DontDestroyOnLoad;
using RCR.Settings.SuperNewScripts.SaveSystem.FileLoaders;
using RCR.Settings.SuperNewScripts.SaveSystem.FileSavers;
using RCR.Settings.SuperNewScripts.SaveSystem.Interfaces;
using RCR.Settings.SuperNewScripts.ScriptableObjects;
using RCR.Settings.SuperNewScripts.World;
using RCR.Settings.SuperNewScripts.World.Interfaces;
using RCR.Systems.ProgressSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace RCR.Settings.SuperNewScripts
{
    [RequireComponent(typeof(Grid))]
    public class Master_World : Singelton<Master_World>
    , IObservableTileMap
    {
        #region Save/Load properties

        private WorldLoader worldLoader;
        
        #region Tiles/Chunks

        
        /// <summary>
        /// Chunk Block Matrix will let me store the ChunkBlock's in a way that can
        /// I can connect them together with Edge's and Lookup adjacent Chunks
        /// </summary>

        public AdjacencyGraph<ChunkBlock, ChunkEdge> AdjacencyGraph { get; private set; }

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
        #endregion
        
        #region EditorOnly

        #endregion

        #region UnityFunctions
        protected override void Awake()
        {
            base.Awake();

            // FileLoader = new ChunkLoader();
            // FileSaver = new ChunkSaver();
            //Declare the Event System so that is up and running at the start on the Loaded Scene
            EventBus = new UnityEventBus(new TokenUtils());


            AdjacencyGraph = new AdjacencyGraph<ChunkBlock, ChunkEdge>();
            //AdjacencyGraph.VertexAdded += UpdateWorldBoundries;
            
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
            InitialData.SetLocationID("AstonMarina");


        }

        private async UniTaskVoid Start()
        {
            worldLoader = new WorldLoader(InitialData.LocationID,  this);
            bool LoadingSuccess = await worldLoader.LoadWorld();
            if (!LoadingSuccess)
            {
                Debug.LogError("Failed TO Load");
            }
            
            //Have Camera GoTo StartPoisition
            //Have the world Have a start position

            // var LoadOperation = await Load();
            // if(LoadOperation)
            //     return;
            // //NEED TO INITIALISE A NEW INSTANCE
            //
            // Value = new AdjacencyMatrix<ChunkBlock>(GameConstants.WorldMaxSize,
            //     GameConstants.WorldMaxSize, true);
            //
            // //Init ChunkOrigins
            // for (int x = 0; x < GameConstants.WorldMaxSize; x++)
            // for (int y = 0; y < GameConstants.WorldMaxSize; y++)
            // {
            //     var Node = Value.GetNode(x, y);
            //     Node.SetOrigin(new Vector2Int(
            //         x * GameConstants.ChunkSize, y * GameConstants.ChunkSize));
            //     Node.SetActive(false);
            // }
            //
            // //Check If Any Overlap
            // for (int x = 0; x < Value.GetLength(0); x++)
            // for (int y = 0; y < Value.GetLength(1); y++)
            //     if (!Value.CheckOtherNodes(x, y,
            //             (C1, C2) => ((C1.Origin.x < C2.Origin.x + GameConstants.ChunkSize) &&
            //                          (C1.Origin.x + GameConstants.ChunkSize > C2.Origin.x) &&
            //                          (C1.Origin.y < C2.Origin.y + GameConstants.ChunkSize) &&
            //                          (C1.Origin.y + GameConstants.ChunkSize > C2.Origin.y))))
            //     {
            //         //Something Overlaps
            //         Debug.LogError("Chunks Overlap");
            //         return;
            //     }
            //
            // var TilemapOperation = await LoadDefaultChunk();
            // if(!TilemapOperation.Succeeded)
            //     return;
            //

        }

        private async UniTaskVoid OnDestroy()
        {
           await worldLoader.SaveWorld();
        }

        #endregion


        private Grid WorldGrid;
        private Tilemap WorldTilemap;
        private PolygonCollider2D WorldBounds;
        //Could use renderer in future but not needed now TODO
        

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
        
        public void UpdateWorldBoundries(ChunkBlock _)
        {
            WorldTilemap.CompressBounds();
            WorldTilemap.ResizeBounds();

            HashSet<Line> edges = new HashSet<Line>(new LineEqualityComparer());
            List<Line> List_edges = new List<Line>();
            foreach (ChunkBlock chunkBlock in AdjacencyGraph.Vertices)
            {
                Line[] ChunkEdges = chunkBlock.GetEdges();
                for (int i = 0; i < ChunkEdges.Length; i++)
                {
                    if (!edges.Add(ChunkEdges[i]))
                    {
                        Line toRemove = edges.First(l => l.StartPosition == ChunkEdges[i].EndPosition
                                                         && ChunkEdges[i].StartPosition == l.EndPosition);
                        List_edges.Remove(toRemove);
                        edges.Remove(toRemove);
                    }
                    else
                    {
                        List_edges.Add(ChunkEdges[i]);
                    }
                }
            }

            Stack<Vector2> path = new Stack<Vector2>();
            path.Push(List_edges[0].StartPosition);
            List_edges.RemoveAt(0);
            while (List_edges.Count != 0)
            {
                Vector2 lastPointAdded = path.Peek();
                bool foundsameEndPoint = List_edges.Any(x => x.EndPosition == lastPointAdded);
                if (!foundsameEndPoint)
                {
                    List_edges.Clear();
                    continue;
                }

                Line sameEndPoint = List_edges.First(x => x.EndPosition == lastPointAdded);
                path.Push(sameEndPoint.StartPosition);
                List_edges.Remove(sameEndPoint);
            }
            
            EventBus.Publish<WorldEvent.WorldBoundriesChanged>(
                new WorldEvent.WorldBoundriesChanged(path.ToArray()), EventArgs.Empty);

        }
        
    }
}