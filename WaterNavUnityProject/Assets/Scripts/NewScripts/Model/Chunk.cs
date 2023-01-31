using Patterns.ObjectPooling;
using Patterns.ObjectPooling.Model;
using RCR.Patterns;
using RCR.Settings.NewScripts.AI;
using RCR.Settings.NewScripts.Geometry;
using UnityEngine;

namespace NewScripts.Model
{
    public class Chunk : BaseModel
    {
        #region Varibles
        // public int ID; //Just in order the are instantiated in
        public Tile[,] tiles;
        public int Width;
        public int Height;

        public int OriginX;
        public int OriginY;

        public Vector2Int ChunkPlayerStartingPoint;
        
        public Vector2Int MatrixID;
        
        public IPool<Tile> TilePool;

        public Line[] Edges;

        public World World;

        public bool HasBeenInitialized = false;

        public AILayer ChunkAiLayer;

        /// <summary>
        /// Active Means the chunk currently has any Tiles or anything inside it. does not mean it's currently visible on screen (could be culled)
        /// </summary>
        public bool Active = false;
        
        #endregion

        #region Properties
        public Tile this[int x, int y]
        {
            get
            {
                if (x > (Width - 1) || y > (Height - 1) || y < 0 || x < 0)
                {
                    Debug.LogError($"Tile {x}, {y} is out of range");
                    return null; //Out of Bounds check
                }
                if (tiles[x, y] == null)
                {
                    tiles[x, y] = TilePool.Request();
                    if (tiles[x, y] == null)
                    {
                        Debug.LogWarning("Potentially the pool has not been pre-warmed");
                        return null;
                    }

                    tiles[x, y].x = x;
                    tiles[x, y].y = y;
                }
                return tiles[x, y];
            }
        }
        #endregion

        #region constructor

        public Chunk()
        {
            TilePool = new Tile_Pool(this);
        }
        #endregion
        
        
      
    }
}