using System;
using RCR.Settings.NewScripts.Entity;
using UnityEngine;

namespace Events.Library.Models.WorldEvents
{
    public class OnEntitySpawned : BaseEvent
    {
        public OnEntitySpawnedArgs args;

        public OnEntitySpawned(OnEntitySpawnedArgs args)
        {
            this.args = args;
        }
        
    }

    public class OnEntitySpawnedArgs : EventArgs
    {
        public EntityType EntityType;
        public Vector2Int EntitySpawnedLocation;

        public OnEntitySpawnedArgs(EntityType type, Vector2Int location)
        {
            EntityType = type;
            EntitySpawnedLocation = location;
        }
    }
}