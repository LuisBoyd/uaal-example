using Patterns.Factory;
using Patterns.Factory.Model;
using RCR.BaseClasses;
using UnityEngine;

namespace RCR.Settings.Map
{
    [RequireComponent(typeof(World_ChunkFactory))]
    [RequireComponent(typeof(Grid))]
    public class WorldMap_Chunks: Singelton<WorldMap_Chunks>
    {
        [SerializeField]
        private WorldSize mChunkSize;

        [SerializeField] 
        private WorldSize mWorldSize;

        private IFactory<WorldChunk> WorldChunk_Factory;

        private WorldChunk[] WorldChunksData;
        
        public WorldChunk this[int x, int y]
        {
            get
            {
                return WorldChunksData[x * GetWorldSize + y];
            }
            set
            {
                WorldChunksData[x * GetWorldSize + y] = value;
            }
        }

        #region Obscur Properties
        public int GetDeaultTextureSize => mChunkSize switch
        {
            WorldSize.x2 => 2,
            WorldSize.x4 => 4,
            WorldSize.x8 => 8,
            WorldSize.x16 => 16,
            WorldSize.x32 => 32,
            WorldSize.x64 => 64,
            WorldSize.x128 => 128,
            WorldSize.x256 => 256,
            WorldSize.x512 => 512,
            WorldSize.x1024 => 1024,
            WorldSize.x2048 => 2048,
            WorldSize.x4096 => 4096,
            _ => 0
        };
        
        public int GetWorldSize => mWorldSize switch
        {
            WorldSize.x2 => 2,
            WorldSize.x4 => 4,
            WorldSize.x8 => 8,
            WorldSize.x16 => 16,
            WorldSize.x32 => 32,
            WorldSize.x64 => 64,
            WorldSize.x128 => 128,
            WorldSize.x256 => 256,
            WorldSize.x512 => 512,
            WorldSize.x1024 => 1024,
            WorldSize.x2048 => 2048,
            WorldSize.x4096 => 4096,
            _ => 0
        };
        #endregion
        protected override void Awake()
        {
            base.Awake();
            WorldChunk_Factory = GetComponent<World_ChunkFactory>();
            InitNewWorldChunks();
        }

        private void InitNewWorldChunks()
        {
            WorldChunksData = new WorldChunk[GetWorldSize * GetWorldSize];
            Vector2Int Position = new Vector2Int(-GetWorldSize / 2, -GetWorldSize / 2);
            for (int x = 0 ; x < GetWorldSize; x++)
            {
                int xPos = Position.x + x;
                for (int y = 0; y < GetWorldSize; y++)
                {
                    int yPos = Position.y + y;
                    this[x, y] = GenerateNewChunk(xPos, yPos);
                }
            }
        }

        private WorldChunk GenerateNewChunk(int x, int y)
        {
            WorldChunk NewChunk = WorldChunk_Factory.Create();
            NewChunk.transform.SetParent(this.transform);
            switch (x)
            {
                case < 0 when y < 0:
                    NewChunk.WorldPosition = new Vector2Int((x * GetDeaultTextureSize), (y * GetDeaultTextureSize));
                    break;
                case < 0:
                    NewChunk.WorldPosition = new Vector2Int((x * GetDeaultTextureSize), Mathf.Abs(y * GetDeaultTextureSize));
                    break;
                default:
                {
                    if (y < 0)
                        NewChunk.WorldPosition =
                            new Vector2Int(Mathf.Abs(x * GetDeaultTextureSize), (y * GetDeaultTextureSize));
                    else
                        NewChunk.WorldPosition = new Vector2Int(Mathf.Abs(x * GetDeaultTextureSize),
                            Mathf.Abs(y * GetDeaultTextureSize));
                    break;
                }
            }
            return NewChunk;
        }
        
    }
}