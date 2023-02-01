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
    /*
     * The Chunk class Stores the big Data for map segments
     */
    public class Chunk : BaseModel
    {
        #region Fields
        public BoundsInt ChunkBounds;
        public Tile[,] tiles;
        public bool HasBeenInitialized = false;
        #endregion

    }
}