#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.U2D;
#endif
using System;
using System.Collections.Generic;
using RCR.Tiled;
using UnityEngine;
using OwnMaths = RCR.Utilities.MathUtils;

namespace WaterNavTiled
{
    public class Map : MonoBehaviour
    {
        public LocalTileSet[] TileSets;

        
        //Class?? for functionality

        public Vector2Int MapSize;
        
        /// <summary>
        /// Tile Width is basically the pixel width and height
        /// </summary>
        public int TileWidth;

#if UNITY_EDITOR
        public SpriteAtlasAsset AtlasAsset;
#endif
    }
}