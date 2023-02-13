using System.Collections.Generic;
using RCR.Settings.NewScripts.Entity;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Events.Library.Models
{
    public class WorldEvent : BaseEvent
    {
        public class BoatSpawnRequest : WorldEvent
        {
            //Should not need more as it's just at one location
        }
        public class CustomerSpawnRequest: WorldEvent
        {
            public int Quantity;
            
            public CustomerSpawnRequest(Boat requestingBoat ,int quantity = 0)
            {
                Quantity = quantity;
                //Get Boat Docked Location to spawn things at TODO
            }
        }
        
        public class CustomerDeSpawnRequest: WorldEvent
        {
            public Customer entity;
            
            public CustomerDeSpawnRequest(Customer entity)
            {
                this.entity = entity;
            }
        }
        
        public class BoatDeSpawnRequest: WorldEvent
        {
            public Boat entity;
            
            public BoatDeSpawnRequest(Boat entity)
            {
                this.entity = entity;
            }
        }
        
        public class WorldTilemapChanged: WorldEvent
        {
            public TileBase[] tiles;
            public Vector3Int[] positions;

            public WorldTilemapChanged(BoundsInt position, TileBase[] tiles)
            {
                this.tiles = tiles;
                List<Vector3Int> positionsList = new List<Vector3Int>();
                foreach (var vector3Int in position.allPositionsWithin)
                {
                    positionsList.Add(vector3Int);
                }
                this.positions = positionsList.ToArray();
            }
            public WorldTilemapChanged(Vector3Int[] positions, TileBase[] tiles)
            {
                this.tiles = tiles;
                this.positions = positions;

            }
        }
        
        public class WorldBoundriesChanged: WorldEvent
        {
            public Vector2[] Boundrypoints;

            public WorldBoundriesChanged(Vector2[] points)
            {
                this.Boundrypoints = points;
            }
        }

    }
}