using System.Collections.Generic;

namespace RCR.Settings.SuperNewScripts
{
    public struct ChunkBlock
    {
        #region Save/Load Properties
        /// <summary>
        /// Tiles Stores all the Datatiles inside this Chunk
        /// </summary>
        public DataTile[,] Tiles;
        #endregion

        public void ReadIn(string[] visualIDs)
        {
            Tiles = new DataTile[GameConstants.ChunkSize, GameConstants.ChunkSize];
            
            for (int x = 0; x < GameConstants.ChunkSize; x++)
            {
                for (int y = 0; y < GameConstants.ChunkSize; y++)
                {
                    Tiles[x,y] = DataTile.Create(visualIDs[(x * GameConstants.ChunkSize) + y]);
                }
            }
        }

    }
}