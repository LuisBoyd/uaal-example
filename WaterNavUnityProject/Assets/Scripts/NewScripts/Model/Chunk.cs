using System.Collections.Generic;
using Patterns.ObjectPooling;
using Patterns.ObjectPooling.Model;
using RCR.Patterns;
using RCR.Settings.NewScripts.AI;
using RCR.Settings.NewScripts.Geometry;
using RCR.Settings.NewScripts.Tilesets;
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

        public PathFindingSystem PathFindingSystem;


        #region SpecialTileLocations
        

        #endregion

        /// <summary>
        /// Active Means the chunk currently has any Tiles or anything inside it. does not mean it's currently visible on screen (could be culled)
        /// </summary>
        public bool Active = false;
        
        #endregion

        #region Properties
        
        #endregion

        #region constructor
        
        #endregion
        
        
      
    }
}