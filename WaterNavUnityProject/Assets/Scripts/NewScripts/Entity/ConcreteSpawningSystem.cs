using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events.Library.Models;
using Events.Library.Models.WorldEvents;
using NewManagers;
using Patterns.ObjectPooling.Model;
using RCR.Settings.NewScripts.Controllers;
using RCR.Settings.NewScripts.TaskSystem;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public class ConcreteSpawningSystem
    {
        private ComponentPool<Customer> CustomerPool;
        private ComponentPool<Boat> boatPool;

        private Vector3Int BoatSpawnLocation;
        private Token On_BoatSpawnerPlacedToken;



        private HashSet<Vector3Int> CustomerSpawnLocations;
        private Token On_CustomerSpawnerPlacedToken;


        public float SpawnTime = 20.0f; //TODO later on these need to be replaced with upgrade's essentially to upgrade
        public int BoatSize = 3;// The frequency and the size of offloading passengers

        public ConcreteSpawningSystem(ref ComponentPool<Boat> boatPool, ref ComponentPool<Customer> CustomerPool,int CustomerQuantity = 20,
            int BoatQuantity = 10)
        {
            this.CustomerPool = CustomerPool;
            this.boatPool = boatPool;
            CustomerPool.PreWarm(CustomerQuantity);
            boatPool.PreWarm(BoatQuantity);
            Active = true;
            CustomerSpawnLocations = new HashSet<Vector3Int>();

            
            On_BoatSpawnerPlacedToken =
                GameManager_2_0.Instance.EventBus.Subscribe<TileEvents.BoatSpawnerTilePlaced>(On_BoatSpawnerPlaced);
            On_CustomerSpawnerPlacedToken = GameManager_2_0.Instance.EventBus.Subscribe<TileEvents.CustomerSpawnerTilePlaced>(On_CustomerSpawnerPlaced);
            
        }
        
        ~ConcreteSpawningSystem()
        {
            Active = false;
            GameManager_2_0.Instance.EventBus.UnSubscribe<TileEvents.BoatSpawnerTilePlaced>(On_BoatSpawnerPlacedToken.TokenId);
            GameManager_2_0.Instance.EventBus.UnSubscribe<TileEvents.CustomerSpawnerTilePlaced>(On_CustomerSpawnerPlacedToken.TokenId);

        }


        public bool Active { get; private set; }

        public Boat SpawnBoat(ref TaskSystem<BoatTask> boatTaskSystem)
        {
            Boat requestedBoat = boatPool.Request();
            requestedBoat.transform.localPosition = BoatSpawnLocation;
            requestedBoat.Setup(boatTaskSystem);
            return requestedBoat;
        }

        public Customer spawnCustomer(ref TaskSystem<CustomerTask> boatTaskSystem)
        {
            return null;
        }

        public void DeSpawnBoat(Boat entity) => boatPool.Return(entity);
        public void DeSpawnCustomer(Customer customer) => CustomerPool.Return(customer);
        

        private void On_BoatSpawnerPlaced(TileEvents.BoatSpawnerTilePlaced evnt, EventArgs args)
        {
            BoatSpawnLocation = evnt.data.TilePlacedLocation;
        }
        private void On_CustomerSpawnerPlaced(TileEvents.CustomerSpawnerTilePlaced evnt, EventArgs args)
        {
            CustomerSpawnLocations.Add(evnt.data.TilePlacedLocation);
        }
    }
}