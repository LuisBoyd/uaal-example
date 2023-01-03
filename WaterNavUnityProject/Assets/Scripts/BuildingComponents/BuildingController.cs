using System;
using System.Collections.Generic;
using RCR.Patterns;
using UnityEngine;

namespace BuildingComponents
{
    public class BuildingController : BaseController<BuildingModel>
    {
        
        public bool AddCustomerToQueue(Rigidbody2D customer) //At the moment work under the assumption that people will not leave the queue
        {
            if (Model.CurrentQueue.Count >= Model.LengthOfQueueUpgrade.Queue_Capcity)
            {
                return false;
            }
            Model.CurrentQueue.Enqueue(customer);
            return true;
        }

        public void ConsumeCustomersFromQueue()
        {
            Model.Customers_Currently_Servicing.Clear();
            List<Rigidbody2D> customers_consumed = new List<Rigidbody2D>();
            for (int i = 0; i < Model.LengthOfQueueUpgrade.Queue_Capcity; i++)
            {
                if (Model.CurrentQueue.TryDequeue(out Rigidbody2D result))
                    customers_consumed.Add(result);
                else
                    break;
            }
            if(customers_consumed.Count <= 0)
                return;
            Model.Customers_Currently_Servicing = customers_consumed;
        }
        
        public float Output()
        {
            Model.m_storedMoney =
                Model.CapcityUpgrade.Calculate_Output(Model.m_storedMoney, Model.CurrentlyServicingQuantity);
            Model.m_storedMoney =  Model.TicketPriceUpgrade.Calculate_Output(Model.m_storedMoney, Model.CurrentlyServicingQuantity);
            Model.m_storedMoney = Model.LengthOfQueueUpgrade.Calculate_Output(Model.m_storedMoney, Model.CurrentlyServicingQuantity);
            Model.m_storedMoney = Model.SpeedOfServiceUpgrade.Calculate_Output(Model.m_storedMoney, Model.CurrentlyServicingQuantity);

            return Model.m_storedMoney;
        }

        public void release_Customers()
        {
            //TODO: release the customers
            throw new NotImplementedException();
        }
        
        
    }
}