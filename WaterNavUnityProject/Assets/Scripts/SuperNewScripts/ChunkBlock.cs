using System.Collections.Generic;
using UnityEngine;

namespace RCR.Settings.SuperNewScripts
{
    public struct ChunkBlock
    {
        #region Save/Load Properties
        /// <summary>
        /// Tiles Stores all the Datatiles inside this Chunk
        /// </summary>
        public DataTile[,] Tiles;
        public Vector2Int Origin { get; private set; }
        public bool Active { get; private set; }
        #endregion

        //No Need for Size as a const is set for the chunks size;
        public void SetOrigin(Vector2Int origin) => Origin = origin;
        public void SetActive(bool state) => Active = state;
        public void SetDataTiles(DataTile[] tiles)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                Tiles[i / tiles.GetLength(0),
                    i % tiles.GetLength(1)] = tiles[i];
            }
        }
        
    }
}