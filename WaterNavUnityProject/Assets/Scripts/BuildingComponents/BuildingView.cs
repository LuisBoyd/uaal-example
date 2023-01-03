using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using RCR.Patterns;
using UnityEngine;

namespace BuildingComponents
{
    public class BuildingView : BaseView<BuildingModel, BuildingController>
    {
        [SerializeField]
        private BezierSpline QueuePath;
        
        

        private class Waitingcustomer
        {
            public int QueuePosition;
            public Rigidbody2D Customer;

            public Waitingcustomer(int queuePosition, Rigidbody2D customer)
            {
                this.QueuePosition = queuePosition;
                this.Customer = customer;
            }
        }
        

        private void Start()
        {
            StartCoroutine(Service());
            DOTween.defaultAutoPlay = AutoPlay.All;
        }

        /// <summary>
        /// A collider has touched the Queue Trigger
        /// </summary>
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<Rigidbody2D>(out Rigidbody2D renderer)) //TODO: change to customer type
            {
                if (Controller.AddCustomerToQueue(renderer))
                {
                    var tween = renderer.transform.DOPath(GetPathToDestination(GetNextFreeSpot()), 3.5f,
                        PathType.Linear,
                        PathMode.TopDown2D, 10, Color.blue);
                }
                //TODO: move customer into queue visually
            }
        }

        private IEnumerator Service()
        {
            while (isActiveAndEnabled && IsValid)
            {
                Controller.ConsumeCustomersFromQueue();
                MoveCustomers();
                yield return new WaitForSecondsRealtime(Model.SpeedOfServiceUpgrade.SpeedOfService);
                //TODO: call Output event on model
                Controller.release_Customers();
            }
        }

        // private void CalculateQueuePoints()
        // {
        //     if(Model.LengthOfQueueUpgrade.Queue_Capcity <= 0)
        //         return;
        //
        //     QueuePoints = new Vector2[Model.LengthOfQueueUpgrade.Queue_Capcity];
        //     float stepSize = 1f / (Model.LengthOfQueueUpgrade.Queue_Capcity);
        //     for (int p = 0, f = 0; f < Model.LengthOfQueueUpgrade.Queue_Capcity; f++, p++)
        //     {
        //         Vector2 Position = QueuePath.GetPoint(p * stepSize);
        //         QueuePoints[f] = Position;
        //     }
        // }

        private int GetNextFreeSpot()
        {
            int value = (Model.LengthOfQueueUpgrade.Queue_Capcity) - Model.CurrentQueue.Count;
            if (value <= 0)
                return -1;
            return value;
        }

        private Vector2[] GetPathToDestination2D(int spot)
        {
            Vector2[]
                path = new Vector2[spot +
                                   1]; //For example let's say we want spot 7 that's actually 8 spots because arrays start at 0
            for (int i = 0; i < spot + 1; i++)
            {
                path[i] = QueuePath.GetPoint((i - 0.0f) / (Model.LengthOfQueueUpgrade.Queue_Capcity) - 0.0f);
            }

            return path;
        }
        
        private Vector3[] GetPathToDestination(int spot)
        {
            Vector3[]
                path = new Vector3[spot +
                                   1]; //For example let's say we want spot 7 that's actually 8 spots because arrays start at 0
            for (int i = 0; i < spot + 1; i++)
            {
                path[i] = QueuePath.GetPoint((i - 0.0f) / (Model.LengthOfQueueUpgrade.Queue_Capcity) - 0.0f);
            }

            return path;
        }

        private void MoveCustomers()
        {
            var Bodies = Model.CurrentQueue.ToArray();
            Model.CurrentQueue.Clear();
            for (int i = 0; i < Bodies.Length; i++)
            {
                var tween = Bodies[i].DOPath(GetPathToDestination2D(GetNextFreeSpot()), 3.5f, PathType.Linear,
                    PathMode.TopDown2D, 10, Color.blue);
                Model.CurrentQueue.Enqueue(Bodies[i]);
            }
        }
    }
}