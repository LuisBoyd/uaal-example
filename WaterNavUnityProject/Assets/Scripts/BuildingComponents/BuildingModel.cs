using System.Collections.Generic;
using RCR.Patterns;
using UnityEngine;

namespace BuildingComponents
{
    public class BuildingModel : BaseModel
    {
        public CapcityUpgrade CapcityUpgrade;
        public LengthOfQueueUpgrade LengthOfQueueUpgrade;
        public SpeedOfServiceUpgrade SpeedOfServiceUpgrade;
        public TicketPriceUpgrade TicketPriceUpgrade;

        public float m_storedMoney;

        public Queue<Rigidbody2D> CurrentQueue; //TODO: change to Customer Type
        public List<Rigidbody2D> Customers_Currently_Servicing;

        public int CurrentlyServicingQuantity
        {
            get => Customers_Currently_Servicing.Count;
        }
        
        public BuildingModel()
        {
            CapcityUpgrade = new CapcityUpgrade(1);
            LengthOfQueueUpgrade = new LengthOfQueueUpgrade(1);
            SpeedOfServiceUpgrade = new SpeedOfServiceUpgrade(1);
            TicketPriceUpgrade = new TicketPriceUpgrade(1);
            m_storedMoney = 0;
            CurrentQueue = new Queue<Rigidbody2D>();
            Customers_Currently_Servicing = new List<Rigidbody2D>();
        }
    }
}