using System.Collections.Generic;
using BuildingComponents.ScriptableObjects;
using RCR.Patterns;
using RCR.Settings.AI;
using RCR.Utilities;
using UnityEngine;

namespace BuildingComponents
{
    public class BuildingModel : BaseModel
    {
        public int Prestige;
        public CapcityUpgrade CapcityUpgrade;
        public LengthOfQueueUpgrade LengthOfQueueUpgrade;
        public SpeedOfServiceUpgrade SpeedOfServiceUpgrade;
        public TicketPriceUpgrade TicketPriceUpgrade;
        public Vector3 WorldPos_buildingExitPoint;
        public float m_storedMoney;

        public ConcreteBuildingObject ConcreteBuildingObject;
        
        
        public Queue<IQueue> CurrentQueue; //TODO: change to Customer Type
        public List<IQueue> Customers_Currently_Servicing;
        public List<IQueue> Customers_Currently_Leaving;
        
        //TODO Event stuff 

        public int CurrentlyServicingQuantity
        {
            get => Customers_Currently_Servicing.Count;
        }
        
        public BuildingModel()
        {
            Prestige = 0; //TODO setting in the settings of the game of max Prestige
            CapcityUpgrade = new CapcityUpgrade(1);
            LengthOfQueueUpgrade = new LengthOfQueueUpgrade(1);
            SpeedOfServiceUpgrade = new SpeedOfServiceUpgrade(1);
            TicketPriceUpgrade = new TicketPriceUpgrade(1);
            m_storedMoney = 0;
            CurrentQueue = new Queue<IQueue>();
            Customers_Currently_Servicing = new List<IQueue>();
            Customers_Currently_Leaving = new List<IQueue>();
            WorldPos_buildingExitPoint = Vector3.zero;
            ConcreteBuildingObject = null;
        }
    }
}