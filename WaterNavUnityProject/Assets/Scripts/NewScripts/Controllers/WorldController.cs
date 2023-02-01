using System;
using System.Collections;
using System.Collections.Generic;
using NewScripts.Model;
using RCR.Patterns;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Threading.Tasks;
using Events.Library.Models;
using Events.Library.Models.WorldEvents;
using NewManagers;
using Patterns.ObjectPooling.Model;
using RCR.Settings.Collections;
using RCR.Settings.Collections.Sorting;
using RCR.Settings.NewScripts.DataStorage.Interfaces;
using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.Geometry;
using RCR.Settings.NewScripts.TaskSystem;
using RCR.Utilities;
using Tile = NewScripts.Model.Tile;

namespace RCR.Settings.NewScripts.Controllers
{
    public class WorldController : BaseController<World>, ILoader<World>
    {

        private AdjacencyMatrix<ChunkController> m_chunkControllers;
        private ConcreteSpawningSystem EntitySpawningSystem;
        private TilemapController m_TilemapController;
        private Token ChunkChangedToken;
        private Token On_EntitySpawned;

        #region Task Systems

        private TaskSystem<BoatTask> BoatTaskSystem;
        private TaskSystem<CustomerTask> CustomerTaskSystem;

        #endregion

        #region SpawnTasks
        private Token On_CustomerDeSpawnRequest;
        private Token On_CustomerSpawnRequest;
        private Token On_BoatDeSpawnRequest;
        private Token On_BoatSpawnRequest;
        #endregion

        #region TileEvents
        private Token OnBoatEndLocationSpawned;
        private Vector3Int BoatDestroyerTileLocation;
        #endregion

        #region Entitys

        private HashSet<IEntity> _EntitesInScene;

        #endregion
        
        #region Public Methods
        public override void Setup(World model)
        {
            base.Setup(model);
        }

        public WorldController()
        {
            ChunkChangedToken = GameManager_2_0.Instance.EventBus.Subscribe<ChunkChanged>(On_ChunkChanged);
            BoatTaskSystem = new TaskSystem<BoatTask>();
            CustomerTaskSystem = new TaskSystem<CustomerTask>();
            _EntitesInScene = new HashSet<IEntity>();
            On_BoatSpawnRequest =
                GameManager_2_0.Instance.EventBus.Subscribe<WorldEvent.BoatSpawnRequest>(OnBoatSpawnRequest);
            On_BoatDeSpawnRequest =
                GameManager_2_0.Instance.EventBus.Subscribe<WorldEvent.BoatDeSpawnRequest>(OnBoatDeSpawnRequest);
            On_CustomerDeSpawnRequest =    GameManager_2_0.Instance.EventBus.Subscribe<WorldEvent.CustomerDeSpawnRequest>(OnCustomerDeSpawnRequest);

            OnBoatEndLocationSpawned = GameManager_2_0.Instance.EventBus.Subscribe<TileEvents.BoatDestroyerTilePlaced>(OnBoatDestoyerTilePlaced);
        }

        ~WorldController()
        {
            GameManager_2_0.Instance.EventBus.UnSubscribe<ChunkChanged>(ChunkChangedToken.TokenId);
            GameManager_2_0.Instance.EventBus.UnSubscribe<WorldEvent.BoatSpawnRequest>(On_BoatSpawnRequest.TokenId);
            GameManager_2_0.Instance.EventBus.UnSubscribe<WorldEvent.BoatDeSpawnRequest>(On_BoatDeSpawnRequest.TokenId);
            GameManager_2_0.Instance.EventBus.UnSubscribe<WorldEvent.CustomerDeSpawnRequest>(On_CustomerDeSpawnRequest.TokenId);
            GameManager_2_0.Instance.EventBus.UnSubscribe<TileEvents.BoatDestroyerTilePlaced>(OnBoatEndLocationSpawned.TokenId);
        }
        public void SetWorldSize(int width, int height, int chunkSize, Transform optionalParent = null)
        {
            m_chunkControllers = new AdjacencyMatrix<ChunkController>(width,height);
            Model.ChunkSize = chunkSize;
            Model.height = height;
            Model.width = width;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    ChunkController cc = m_chunkControllers.CreateNode(new ChunkController(), x, y);
                }
            }
            SetUpTilemap(width,height,chunkSize, optionalParent);
            SetChunkPositionUniform(chunkSize);
            if (CheckIfAnyChunksOverlap())
            {
                Debug.LogError("Some Chunks Overlap");
                return;
            }
        }

        public void InitWorldComponents( ComponentPool<Boat> boatPool,  ComponentPool<Customer> CustomerPool)
        {
            EntitySpawningSystem = new ConcreteSpawningSystem( ref boatPool ,ref CustomerPool);
        }

        /// <summary>
        /// Calculate the Total Tiles in the entire world
        /// </summary>
        /// <returns>returns the total amount of tiles avalible in the world. DOES NOT MEAN
        /// that the chunks where those tiles are stored are active</returns>
        public int GetTileCount()
        {
            int count = 0;
            for (int x = 0; x < Model.width; x++)
            {
                for (int y = 0; y < Model.height; y++)
                {
                    // count += Model.Chunks.GetNode(x,y). * Model.Chunks[x, y].Height;
                    // Chunk xy = Model.Chunks.GetNode(x, y);
                    // count += xy.Width * xy.Height;
                    // xy = null;
                }
            }
            return count;
        }

        public ChunkController GetChunkController(int x, int y) => m_chunkControllers.GetNode(x, y);
        public ChunkController[] GetChunkControllers() => m_chunkControllers.GetNodes();

        /// <summary>
        /// Gets all Adjacent ChunkControllers Based on Index X,Y
        /// </summary>
        /// <returns>Array of Adjacent ChunkControllers</returns>
        public ChunkController[] GetAdjacentChunks(int x, int y)
        {
            var edges = m_chunkControllers.GetEdges(x, y);
            List<ChunkController> controllers = new List<ChunkController>(); 
            foreach (var edge in edges)
                controllers.Add(edge.EdgeEnd);
            return controllers.ToArray();
        }

        public void CopyTilesToChunk(int x, int y, ref Tilemap tilemap, ref Tilemap ToCopy)
        {
            ChunkController cc = m_chunkControllers.GetNode(x, y);
            if(!cc.SetChunkVisuals( ref ToCopy))
                return;
            TryToSetPaths(x,y);
        }
        public void CopyTilesToChunk(int x, int y, ref Tilemap tilemap, ref Tilemap ToCopy,
            BoundsInt areaToCopy)
        {
            ChunkController cc = m_chunkControllers.GetNode(x, y);
            if(!cc.SetChunkVisuals(ref ToCopy, areaToCopy))
                return;
            TryToSetPaths(x,y);
        }
        public void CopyTilesToChunk(int x, int y, ref Tilemap tilemap, ref Tilemap ToCopy,
            BoundsInt areaToCopy, BoundsInt areaToPaste)
        {
            ChunkController cc = m_chunkControllers.GetNode(x, y);
            if(!cc.SetChunkVisuals(ref ToCopy, areaToCopy, areaToPaste))
                return;
            TryToSetPaths(x,y);
        }

        public void UpdateWorldBoundries()
        {
            m_TilemapController.CompressBounds();
            m_TilemapController.ResizeBounds();
            var adjacent_chunks = GetChunkControllers();
            HashSet<Line> edges = new HashSet<Line>(new LineEqualityComparer());
            List<Line> edges_l = new List<Line>(); 
            
            //Algorithm Idea https://stackoverflow.com/questions/33472951/outline-circumference-polygon-extraction-from-geometry-constructed-from-equal
            
            foreach (var chunkController in adjacent_chunks)
            {
                // if(!chunkController.IsChunkActive())
                //     continue;
                Line[] chunkEdges = chunkController.GetChunkEdges();
                for (int i = 0; i < chunkEdges.Length; i++)
                {
                    if (!edges.Add(chunkEdges[i]))
                    {
                       Line toRemove = edges.First(l => l.StartPosition == chunkEdges[i].EndPosition
                                               && chunkEdges[i].StartPosition == l.EndPosition);
                       edges_l.Remove(toRemove);
                       edges.Remove(toRemove);
                    }
                    else
                    {
                        edges_l.Add(chunkEdges[i]);
                    }
                }
            }

            // Vector2[] path = new Vector2[edges.Count];
            Stack<Vector2> path = new Stack<Vector2>();
            path.Push(edges_l[0].StartPosition);
            // path[0] = edges_l[0].StartPosition;
            edges_l.RemoveAt(0);

            while (edges_l.Count != 0)
            {
                Vector2 lastPointAdded = path.Peek();
                bool FoundsameEndPoint = edges_l.Any(x => x.EndPosition == lastPointAdded);
                if (!FoundsameEndPoint)
                {
                    edges_l.Clear();
                }
                else
                {
                    Line sameEndPoint = edges_l.First(x => x.EndPosition == lastPointAdded);
                    path.Push(sameEndPoint.StartPosition);
                    edges_l.Remove(sameEndPoint);
                }
            }
            

            //Dependent on performance have a hashset ordered by LINQ
            // LBUtilities
            // var orderBy = edges.OrderBy(l => l, new LineOrderByConnectivity());
            // Vector2[] LineStartPoints = orderBy.Select(l => l.StartPosition).ToArray();
            // worldBoundries.SetPath(0, LineStartPoints);
            //Vector2[] shape = LBUtilities.graham_Scan(edges.Select(l => l.StartPosition).ToArray());
            GameManager_2_0.Instance.EventBus.Publish<WorldBoundsChanged>(
                new WorldBoundsChanged(), new WorldBoundsChangedArgs(
                    path.ToArray(), false,Model.WorldPlayerStartPoint));
            path = null;
            edges_l.Clear();
            edges_l = null;
            adjacent_chunks = null;
            edges.Clear();
            edges = null;
        }

        public IEnumerator SpawningLoop() //TODO will need some value to determine rate of spawning for now just gonna set it to 10seconds
        {
            yield return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check the Bounds of the 2 Chunks the controllers represent
        /// </summary>
        /// <returns>True if they do not overlap, False if they do Overlap</returns>
        private bool CheckIfChunkOverlap(ChunkController controllerA, ChunkController controllerB)
        {
            Chunk a = controllerA.GetChunk();
            Chunk b = controllerB.GetChunk();
            
            // if ((a.OriginX < b.OriginX + b.Width) &&
            //     (a.OriginX + a.Width > b.OriginX) &&
            //     (a.OriginY < b.OriginY + b.Height) &&
            //     (a.OriginY + a.Height > b.OriginY))
            // {
            //     Debug.LogWarning("Chunk a: \n" +
            //                      $"Origin: {a.OriginX},{a.OriginY}, Width: {a.Width}, Height: {a.Height} \n" +
            //                      $"Chunk b: \n" +
            //                      $"Origin: {b.OriginX},{b.OriginY}, Width: {b.Width}, Height: {b.Height}");
            //     return !true;
            // }

            return !false;
        }

        /// <summary>
        /// Checks If any Registered Chunks Overlap Each other
        /// </summary>
        /// <returns>True if one chunk overlaps with another, False if no chunks overlap with any</returns>
        private bool CheckIfAnyChunksOverlap()
        {
            // for (int i = 0; i < m_chunkControllers.GetLength(); i++)
            // {
            //     Chunk a = this[i].GetChunk();
            //     for (int j = 0; j < m_chunkControllers.GetLength(); j++)
            //     {
            //         if (j != i)
            //         {
            //             Chunk b = this[j].GetChunk();
            //             if (CheckIfChunkOverlap(a, b))
            //             {
            //                 Debug.Log($"Chunk {a.ID} and Chunk {b.ID} Overlap each other");
            //                 return true;
            //             }
            //         }
            //     }
            // }

            for (int x = 0; x < m_chunkControllers.GetLength(0); x++)
            {
                for (int y = 0; y < m_chunkControllers.GetLength(1); y++)
                {
                    if (!m_chunkControllers.CheckOtherNodes(x, y, CheckIfChunkOverlap))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        private void SetChunkPositionUniform(int chunkSize)
        {
            for (int x = 0; x < Model.width; x++)
            {
                for (int y = 0; y < Model.height; y++)
                {
                    // m_chunkControllers[x,y].SetChunkData(Model,x * chunkSize,
                    //     y * chunkSize, chunkSize, chunkSize);
                    m_chunkControllers.GetNode(x,y).SetChunkData(x * chunkSize,y * chunkSize,
                        y * chunkSize, chunkSize);
                }
            }
        }

        /// <summary>
        /// Attempts to set paths to other active chunks if they are active if not then it does nothing
        /// </summary>
        private void TryToSetPaths(int x, int y)
        {
            m_chunkControllers.CreatePath(x,y,x + 1,y, ChunksActive); //Left
            m_chunkControllers.CreatePath(x,y,x - 1,y, ChunksActive); //Right
            m_chunkControllers.CreatePath(x,y,x,y + 1, ChunksActive); //Up
            m_chunkControllers.CreatePath(x,y,x,y - 1, ChunksActive); //Down
        }

        /// <summary>
        /// Check to see if both chunks are active
        /// </summary>
        /// <returns>True if both chunks are active, False if one or both are Inactive</returns>
        private bool ChunksActive(ChunkController a, ChunkController b)
        {
            // return a.GetChunk().Active && b.GetChunk().Active;
            return false;
        }

        private void SetUpTilemap(int width, int height, int chunkSize,Transform optionalParent = null)
        {
            GameObject TilemapGameObject = new GameObject("_Tilemap_world");
            TilemapGameObject.transform.SetParent(optionalParent);
            TilemapRenderer tilemapRenderer = TilemapGameObject.AddComponent<TilemapRenderer>();
            Tilemap tilemap = TilemapGameObject.gameObject.GetComponent<Tilemap>();
            //collider2D.usedByComposite = true;
            tilemapRenderer.mode = TilemapRenderer.Mode.Chunk;
            tilemap.origin = Vector3Int.zero;
            tilemap.size = new Vector3Int(width * chunkSize, height * chunkSize, 1);
            tilemap.ResizeBounds();

            m_TilemapController = new TilemapController();
            m_TilemapController.SetTilemapData(tilemap);
        } 
        
        private void On_ChunkChanged(ChunkChanged evnt, EventArgs args) => UpdateWorldBoundries();
        
        private void OnBoatSpawnRequest(WorldEvent.BoatSpawnRequest evnt, EventArgs args)
        {
            _EntitesInScene.Add(EntitySpawningSystem.SpawnBoat(ref BoatTaskSystem));
        }
        private void OnBoatDeSpawnRequest(WorldEvent.BoatDeSpawnRequest evnt, EventArgs arg)
        {
            _EntitesInScene.Remove(evnt.entity);
            EntitySpawningSystem.DeSpawnBoat(evnt.entity);
        }
        private void OnCustomerDeSpawnRequest(WorldEvent.CustomerDeSpawnRequest evnt, EventArgs arg)
        {
            _EntitesInScene.Remove(evnt.entity);
            EntitySpawningSystem.DeSpawnCustomer(evnt.entity);
        }
        
        private void OnBoatDestoyerTilePlaced(TileEvents.BoatDestroyerTilePlaced evnt, EventArgs arg2)
        {
            
        }
        #endregion

        #region Loading/Saving

        public IFileReader FileReader { get; }
        public Task<World> ReconstructObject()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}