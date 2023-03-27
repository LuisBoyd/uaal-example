using System.Collections;
using System.Collections.Generic;
using RCRCoreLib.Core.Tiles.TilemapSystem;
using UnityEngine;

namespace RCRCoreLib.Core.AI
{
    public abstract class AIGrid
    {
        //Structures should have NodePoint's that way I can cache paths between places

        protected AIGrid()
        {
            map = new Dictionary<Vector2Int, PathFindNode>();
        }

        protected IDictionary<Vector2Int, PathFindNode> map; //Uniform Cost Node Graph.

        public abstract void ChangeMap(Vector2Int coord, WorldTilePathAffector pathAffector); //Change the map based on the effect flag passed in.
        public abstract void ChangeMap(Vector2Int coord, bool passable); //Change map based on a bool.
        public abstract void ChangeMap(IDictionary<Vector2Int, WorldTilePathAffector> mapChangeCollection); //Change the map based on effect flag's but in batches.
        public abstract void ChangeMap(IDictionary<Vector2Int, bool> mapChangeCollection); //Change the map based on bool but in batches.

    }
}