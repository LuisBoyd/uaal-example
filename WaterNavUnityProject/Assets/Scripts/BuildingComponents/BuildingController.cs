using System;
using System.Collections.Generic;
using DataStructures;
using RCR.Patterns;
using RCR.Settings.AI;
using UnityEngine;

namespace BuildingComponents
{
    public class BuildingController : BaseController<BuildingModel>
    {
        public void AddCustomerToQueue(IQueue customer) => Model.CurrentQueue.Enqueue(customer);
        

        public void ConsumeCustomersFromQueue()
        {
            if (!CheckIfCanConsume())
                return;
            List<IQueue> customers_consumed = new List<IQueue>();
            for (int i = 0; i < Model.CapcityUpgrade.ServiceCapacity; i++)
            {
                if (Model.CurrentQueue.TryDequeue(out IQueue result))
                {
                    result.ProgressThroughQueue = 0f;
                    customers_consumed.Add(result);
                    result.GameObject.SetActive(false);
                }
                else
                {
                    break;
                }
            }
            if(customers_consumed.Count <= 0)
                return;
            Model.Customers_Currently_Servicing = customers_consumed;
        }

        private bool CheckIfCanConsume()
        {
            if (Model.CurrentQueue.TryPeek(out IQueue customer))
                return customer.StopPathProgression;

            return false;
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

        public void release_Customers(BezierSpline spline, float duration)
        {
            foreach (IQueue customer in Model.Customers_Currently_Servicing)
            {
                customer.GameObject.transform.position = Model.WorldPos_buildingExitPoint;
                customer.GameObject.SetActive(true); //TODO remember I am turning the game object off and on when they are serviced
                customer.On_QueueEntered(spline, duration);
                customer.LeaveQueue_Serviced();
            }
            Model.Customers_Currently_Servicing.Clear();
        }
    }
}