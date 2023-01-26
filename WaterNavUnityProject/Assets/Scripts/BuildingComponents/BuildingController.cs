using System;
using System.Collections;
using System.Collections.Generic;
using BuildingComponents.ScriptableObjects;
using DataStructures;
using RCR.Patterns;
using RCR.Settings.AI;
using UnityEngine;
using Bezier;

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
                    customers_consumed.Add(result);
                    result.EnterService();
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

        // public IEnumerator release_Customers(BezierSpline spline, float duration)
        // {
        //     foreach (IQueue customer in Model.Customers_Currently_Servicing)
        //     {
        //        //TODO remember I am turning the game object off and on when they are serviced
        //         customer.On_QueueEntered(spline, duration);
        //         customer.LeaveQueue_Serviced(Model.WorldPos_buildingExitPoint);
        //         yield return new WaitForSecondsRealtime(0.3f);
        //     }
        //     Model.Customers_Currently_Servicing.Clear();
        // }

        public void SetConcreteBuildingType(ConcreteBuildingObject obj)
        {
            Model.ConcreteBuildingObject = obj;
        }
    }
}