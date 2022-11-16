﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DataStructures
{
    [System.Serializable]
    public class MapData
    {
        public string Region;
        public string MapID;
        public int[] MapIdArray;

        /// <summary>
        /// X (rows) : are the Map Sections so the Value 1 and 50 would mean "Map Section 1, 50th Tile in the Flat Array". <br/>
        /// Y (Columns) : are Individual Tiles that correspond to some enum value to determine looks
        /// </summary>
        public byte[] MapByteStructure;
        
        /// <summary>
        /// MapSize_sqr is the square root of MapSize.
        /// </summary>
        public float MapSize_sqr;
        
        /// <summary>
        /// Size of the map if it were a Total Count of Map Sections (Size) is counted by how many Individual Map Sections are present.
        /// </summary>
        public int MapSize;
    }
}