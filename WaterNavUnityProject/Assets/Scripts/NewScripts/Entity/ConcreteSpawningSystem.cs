using System;
using System.Collections.Generic;
using System.Linq;
using Events.Library.Models;
using Events.Library.Models.WorldEvents;
using NewManagers;
using Patterns.ObjectPooling.Model;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public class ConcreteSpawningSystem : IEntitySpawningSystem
    {

        private ComponentPool<Entity> _EntityPool;
        private Dictionary<Vector2Int, EntityType> _EntitySpawnerLocations;

        public ConcreteSpawningSystem(ComponentPool<Entity> pool, int warmUpQuantity = 100)
        {
            _EntityPool = pool;
            _EntityPool.PreWarm(warmUpQuantity);
            _EntitySpawnerLocations = new Dictionary<Vector2Int, EntityType>();
            Active = true;
            On_spawnerPlacement = GameManager_2_0.Instance.EventBus.Subscribe<OnSpawnerPlaced>(On_spawnerPlaced);
        }
        ~ConcreteSpawningSystem()
        {
            Active = false;
            GameManager_2_0.Instance.EventBus.UnSubscribe<OnSpawnerPlaced>(On_spawnerPlacement.TokenId);
        }


        public bool Active { get; private set; }
        public Token On_spawnerPlacement { get; }


        public void Spawn(EntitySpawningOptions options = default)
        {
            if(_EntitySpawnerLocations.Count <= 0)
                return;
            Entity e = _EntityPool.Request();
            Vector2Int spawnLocation = _EntitySpawnerLocations.First(kp =>
                kp.Value == EntityType.Boat).Key;
            e.ChangeEntityBehaviour(new BoatController(), new BoatAttributes());
            e.transform.position = new Vector3(spawnLocation.x + .5f, spawnLocation.y + .5f);
            e.transform.SetParent(options.Parent);
            //TODO Add rotation
        }

        public void Spawn(Vector2Int requestingLocation, EntitySpawningOptions options = default)
        {
            if(_EntitySpawnerLocations.Count <= 0)
                return;
            Entity e = _EntityPool.Request();
            Vector2Int spawnLocation;
            switch (_EntitySpawnerLocations[requestingLocation])
            {
                default:
                    Debug.LogWarning($"Can't Spawn A entity with no behavior");
                    _EntityPool.Return(e);
                    break;
                case EntityType.Boat:
                    e.ChangeEntityBehaviour(new BoatController(), new BoatAttributes());
                    break;
                case EntityType.Customer:
                    e.ChangeEntityBehaviour(new CustomerController(), new CustomerAttributes());
                    break;
            }
            e.transform.position = new Vector3(requestingLocation.x + .5f, requestingLocation.y + .5f);
            e.transform.SetParent(options.Parent);
            //TODO Add rotation
        }

        public void Despawn(Entity e)
        {
            _EntityPool.Return(e);
        }

        public void StopPauseSystem()
        {
            Active = false;
        }

        public void ResumeSystem()
        {
            Active = true;
        }

        private void On_spawnerPlaced(OnSpawnerPlaced envt, EventArgs args)
        {
            if (envt.Args.Removed)
            {
                if(_EntitySpawnerLocations.ContainsKey(envt.Args.Position))
                    _EntitySpawnerLocations.Remove(envt.Args.Position);
            }
            else
            {
                if (_EntitySpawnerLocations.ContainsValue(EntityType.Boat))
                {
                    //If a boat Location Exists already remove it first
                    _EntitySpawnerLocations.Remove(_EntitySpawnerLocations.First(kp => kp.Value == EntityType.Boat).Key);
                }
                if(!_EntitySpawnerLocations.ContainsKey(envt.Args.Position))
                    _EntitySpawnerLocations.Add(envt.Args.Position, envt.entityType);
            }
        }
    }
}