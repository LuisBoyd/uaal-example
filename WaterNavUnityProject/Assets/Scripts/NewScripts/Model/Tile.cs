using System.Collections;
using System.Collections.Generic;
using RCR.Patterns;
using RCR.Settings.NewScripts.Tilesets;
using UnityEngine;

namespace NewScripts.Model
{
    public class Tile : BaseModel
    {
        #region Varibles

        //Enum For looks too
        public LogicTile.TileIdentifiers Identifier;
        public LogicTile.VisualIndicator VisualIndicator;
        
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

        public Tile(Chunk c, LogicTile tile, Vector2Int LocalLocation)
        {
            this.Chunk = c;
            Identifier = tile.Identifiers;
            VisualIndicator = tile.Indicator;
            this.x = LocalLocation.x;
            this.y = LocalLocation.y;
        }
        #endregion
        
    }
}
