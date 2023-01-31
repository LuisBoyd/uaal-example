using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events.Library.Models;
using Events.Library.Models.WorldEvents;
using NewManagers;
using NewScripts.Model;
using Patterns.ObjectPooling.Model;
using RCR.Patterns;
using RCR.Settings.Collections.Sorting;
using RCR.Settings.NewScripts.Controllers;
using RCR.Settings.NewScripts.Geometry;
using RCR.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using Random = UnityEngine.Random;
#endif


namespace RCR.Settings.NewScripts.View
{
    [RequireComponent(typeof(EntityPool))]
    [RequireComponent(typeof(Grid))]
    public class WorldView: BaseView<World, WorldController>
    {
        public enum ChunkSize
        {
            x2,
            x4,
            x8,
            x16,
            x32,
            x64,
            x128,
            x256,
            x512,
            x1024,
            x2048,
            x4096

        }
        public enum WorldSize
        {
            x2,
            x4,
            x6,
            x8,
            x10,
            x12
        }

        #region varibles
        [SerializeField] [Tooltip("Width and height in cell size")]
        private ChunkSize Chunk_Size;
        [SerializeField] [Tooltip("Width and height in Chunks")]
        private WorldSize World_Size;
        private Tilemap worldTilemap;
        private static PolygonCollider2D worldCollider;
        private TilemapCollider2D TilemapCollider2D;
        [SerializeField] 
        private Tilemap TestTilemap;

        private List<Vector2> BoundingPoints;
        #endregion
        

        #region properties
        /// <summary>
        /// Get's the world size squared
        /// </summary>
        public int GetChunkSize => Chunk_Size switch
        {
            ChunkSize.x2 => 2,
            ChunkSize.x4 => 4,
            ChunkSize.x8 => 8,
            ChunkSize.x16 => 16,
            ChunkSize.x32 => 32,
            ChunkSize.x64 => 64,
            ChunkSize.x128 => 128,
            ChunkSize.x256 => 256,
            ChunkSize.x512 => 512,
            ChunkSize.x1024 => 1024,
            ChunkSize.x2048 => 2048,
            ChunkSize.x4096 => 4096,
            _ => 0
        };
        public int GetWorldSize => World_Size switch
        {
            WorldSize.x2 => 2,
            WorldSize.x4 => 4,
            WorldSize.x6 => 6,
            WorldSize.x8 => 8,
            WorldSize.x10 => 10,
            WorldSize.x12 => 12,
            _ => 0
        };

        public static PolygonCollider2D WorldBoundsCollider
        {
            get => worldCollider;
        }
        #endregion

        #region Unity Functions

        #region private methods
        



        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            BoundingPoints = new List<Vector2>();
            Controller.SetWorldSize(GetWorldSize, GetWorldSize,
                GetChunkSize, this.transform);
            Controller.InitWorldComponents(GetComponent<ComponentPool<Entity.Entity>>());
        }
        

        private void OnEnable()
        {
#if UNITY_EDITOR
            Debug.Log($"The Current World Tile Count: {Controller.GetTileCount()}");      
#endif
        }

        private IEnumerator Start()
        {
            StartCoroutine(Controller.SpawningLoop());
            Controller.GetChunkController((GetWorldSize-1)/2, (GetWorldSize-1)/2).SetChunkVisuals( ref TestTilemap);
            yield return new WaitForSecondsRealtime(20.0f);
            Controller.GetChunkController(1, 0).SetChunkVisuals(ref TestTilemap);
            // yield return new WaitForSecondsRealtime(7.0f);
            // Controller.GetChunkController(0, 1).SetChunkVisuals(ref TestTilemap);
            // yield return new WaitForSecondsRealtime(7.0f);
            // Controller.GetChunkController(1, 2).SetChunkVisuals(ref TestTilemap);
            // yield return new WaitForSecondsRealtime(7.0f);
            // Controller.GetChunkController(0, 2).SetChunkVisuals(ref TestTilemap);
        
        }

        #endregion

        #region Only-Editor
#if UNITY_EDITOR
        [Header("Editor Only Values")]
        [SerializeField] 
        private Vector3 DebugCubeSize;

        [SerializeField] 
        private Vector3 DebugCubeOffset;
        
        
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                foreach (ChunkController chunkController in Controller.GetChunkControllers())
                {
                    Chunk chunk = chunkController.GetChunk();
                    Gizmos.color = chunkController.DebugColor;
                    Gizmos.DrawCube(new Vector3(chunk.OriginX, chunk.OriginY) + transform.position + DebugCubeOffset,
                        DebugCubeSize); //Min
                    Gizmos.DrawCube(
                        new Vector3(chunk.OriginX + chunk.Width, chunk.OriginY + chunk.Height) + transform.position + DebugCubeOffset,
                       DebugCubeSize); //Max
                    Gizmos.DrawCube(new Vector3(chunk.OriginX + chunk.Width, chunk.OriginY) + transform.position + DebugCubeOffset,
                        DebugCubeSize); //Max x
                    Gizmos.DrawCube(new Vector3(chunk.OriginX, chunk.OriginY + chunk.Height) + transform.position + DebugCubeOffset,
                       DebugCubeSize); //Max y
                }
            }
        }

#endif
        #endregion
    }
}