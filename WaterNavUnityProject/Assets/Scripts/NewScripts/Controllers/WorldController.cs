using NewScripts.Model;
using RCR.Patterns;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Tile = NewScripts.Model.Tile;

namespace RCR.Settings.NewScripts.Controllers
{
    public class WorldController : BaseController<World>
    {

        private ChunkController[,] m_chunkControllers;

        private ChunkController this[int value]
        {
            get
            {
                int y = value % m_chunkControllers.GetLength(1);
                int x = value / m_chunkControllers.GetLength(0);
                
                if (x < 0 || y < 0 || x >= m_chunkControllers.GetLength(0) || y >= m_chunkControllers.GetLength(1))
                    return null;

                return m_chunkControllers[x, y];
            }
        }
        

        #region Public Methods
        public override void Setup(World model)
        {
            base.Setup(model);
        }
        public Tilemap SetWorldSize(int width, int height, int chunkSize)
        {
            m_chunkControllers = new ChunkController[width, height];
            Model.Chunks = new Chunk[width, height];
            Model.ChunkSize = chunkSize;
            Model.height = height;
            Model.width = width;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    m_chunkControllers[x, y] = new ChunkController();
                    m_chunkControllers[x,y].PreWarmTiles(chunkSize * chunkSize);
                    Model.Chunks[x, y] = m_chunkControllers[x, y].GetChunk();
                    Model.Chunks[x, y].ID = (x * height) + y;
                }
            }
            
            SetChunkPositionUniform(chunkSize);
            if (CheckIfAnyChunksOverlap())
            {
                Debug.LogError("Some Chunks Overlap");
                return null;
            }
            
            TilemapRenderer tilemapRenderer = new GameObject("_Tilemap_world").AddComponent<TilemapRenderer>();
            Tilemap tilemap_gamObject = tilemapRenderer.gameObject.GetComponent<Tilemap>();
            tilemapRenderer.mode = TilemapRenderer.Mode.Chunk;
            tilemap_gamObject.origin = Vector3Int.zero;
            tilemap_gamObject.size = new Vector3Int(width * chunkSize, height * chunkSize, 1);
            tilemap_gamObject.ResizeBounds();
            return tilemap_gamObject;
        }

        public int GetTileCount()
        {
            int count = 0;
            
            for (int x = 0; x < Model.width; x++)
            {
                for (int y = 0; y < Model.height; y++)
                {
                    count += Model.Chunks[x, y].Width * Model.Chunks[x, y].Height;
                }
            }

            return count;
        }

        public ChunkController GetCunkController(int x, int y) => m_chunkControllers[x, y];
        #endregion

        #region Private Methods

        private bool CheckIfChunkOverlap(Chunk a, Chunk b)
        {
            if ((a.OriginX < b.OriginX + b.Width) &&
                (a.OriginX + a.Width > b.OriginX) &&
                (a.OriginY < b.OriginY + b.Height) &&
                (a.OriginY + a.Height > b.OriginY))
            {
                Debug.LogWarning("Chunk a: \n" +
                                 $"Origin: {a.OriginX},{a.OriginY}, Width: {a.Width}, Height: {a.Height} \n" +
                                 $"Chunk b: \n" +
                                 $"Origin: {b.OriginX},{b.OriginY}, Width: {b.Width}, Height: {b.Height}");
                return true;
            }

            return false;
        }

        private bool CheckIfAnyChunksOverlap()
        {
            for (int i = 0; i < m_chunkControllers.Length; i++)
            {
                Chunk a = this[i].GetChunk();
                for (int j = 0; j < m_chunkControllers.Length; j++)
                {
                    if (j != i)
                    {
                        Chunk b = this[j].GetChunk();
                        if (CheckIfChunkOverlap(a, b))
                        {
                            Debug.Log($"Chunk {a.ID} and Chunk {b.ID} Overlap each other");
                            return true;
                        }
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
                    m_chunkControllers[x,y].SetChunkData(Model,x * chunkSize,
                        y * chunkSize, chunkSize, chunkSize);
                }
            }
        }
        
        #endregion
    }
}