﻿using System;
using Patterns.ObjectPooling;
using Patterns.ObjectPooling.Model;
using RCR.Patterns;
using RCR.Settings.Collections;
using UnityEngine;

namespace NewScripts.Model
{
    public class World : BaseModel
    {

        #region varibles
        
        /// <summary>
        /// The Width of the world in Chunks
        /// </summary>
        public int width;
        /// <summary>
        /// The Height of the world in chunks
        /// </summary>
        public int height;

        /// <summary>
        /// The Size of Chunks in tiles squared
        /// </summary>
        public int ChunkSize;

        /// <summary>
        /// The Width of the world in Tiles
        /// </summary>
        public int TileWidth => ChunkSize * width;
        /// <summary>
        /// The height of the world in Tiles
        /// </summary>
        public int TileHeight => ChunkSize * height;

        public Vector2Int WorldPlayerStartPoint;
        
        public bool setWorldPoint = false;
        #endregion

        #region properties
        
        #endregion

        #region Constructors
        public World()
        {
        }
        #endregion

    }
}