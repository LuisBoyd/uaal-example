using System;
using RCR.Settings.NewScripts.Tilesets;
using UnityEngine;

namespace Events.Library.Models
{
    public class TileEvents: BaseEvent
    {
        
        public class SpecialTilePlacedArgs: EventArgs
        {
            public Vector3Int TilePlacedLocation;
            public SpecialTilePlacedArgs(Vector3Int point) => TilePlacedLocation = point;
        }
        
        public class BoatSpawnerTilePlaced: TileEvents
        {
            public SpecialTilePlacedArgs data;
            public BoatSpawnerTilePlaced(SpecialTilePlacedArgs arg) => data = arg;
        }
        
        public class CustomerSpawnerTilePlaced: TileEvents
        {
            public SpecialTilePlacedArgs data;
            public CustomerSpawnerTilePlaced(SpecialTilePlacedArgs arg) => data = arg;
        }
        
        public class BoatDestroyerTilePlaced: TileEvents
        {
            public BoatDestroyerTile Tile;

            public BoatDestroyerTilePlaced(BoatDestroyerTile arg)
            {
                Tile = arg;
            }
        }
    }
}