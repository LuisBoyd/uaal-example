using System.Collections;
using System.Collections.Generic;
using RCR.Patterns;
using UnityEngine;

namespace NewScripts.Model
{
    public class Tile : BaseModel
    {
        public enum TileType
        {
            Empty_Water,
            Water,
            Ground
        }

        #region Varibles
        private TileType type = TileType.Empty_Water;

        private LooseObject looseObject;
        private InstalledObject installedObject;
        
        private Chunk Chunk;
        public int x;
        public int y;
        #endregion

        #region constructor

        public Tile(Chunk chunk)
        {
            this.Chunk = chunk;
            this.x = 0;
            this.y = 0;
        }
        #endregion
        
    }
}
