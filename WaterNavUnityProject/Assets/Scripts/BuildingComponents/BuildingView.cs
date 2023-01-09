using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using RCR.Patterns;
using RCR.Settings;
using RCR.Settings.AI;
using RCR.Utilities;
using UnityEngine;

namespace BuildingComponents
{
    public class BuildingView : BaseView<BuildingModel, BuildingController>
    {
        [SerializeField]
        private BezierSpline QueuePath;

        [SerializeField] 
        private BezierSpline ExitPath;

        [SerializeField] 
        private Transform QueueBarrier;
        
        /// <summary>
        /// if true the Entrance to this building force will be applied in the right direction if
        /// not then the left and the exit force will be always be the opposite.
        /// </summary>
        public bool ApplyEntranceForceRight;

        private Vector2 m_queueEndPoint;

        private bool EndOfQueueOccupied = false;

        public event EventHandler on_TakenInNewCustomers;
        public event EventHandler on_endOfQueueOccupied;

        private static float Queue_Duration = 10f;

        private Vector2 StartOfLineSegment = Vector2.zero;
        private Vector2 EndOfLineSegment = Vector2.zero;
        private float t = 0.0f;
        
        private void Start()
        {
            //StartCoroutine(Service());
            //DOTween.defaultAutoPlay = AutoPlay.All;
            if(!InitQueue())
                return; //Maybe Set the entire Object Inactive

            StartCoroutine(Service());

        }

        /// <summary>
        /// A collider has touched the Queue Trigger
        /// </summary>
        private void OnTriggerEnter2D(Collider2D col)
        {
            EndOfQueueOccupied = true;
            StartCoroutine(CheckEndOfQueue());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            EndOfQueueOccupied = false;
        }

        private IEnumerator CheckEndOfQueue()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            if (EndOfQueueOccupied)
            {
                on_endOfQueueOccupied?.Invoke(this,EventArgs.Empty);
            }
        }

        public void AddCustomerToQueue(IQueue customer) => Controller.AddCustomerToQueue(customer);

        public void RemoveCustomerFromBuilding(IQueue customer)
        {
            if (Model.Customers_Currently_Leaving.Contains(customer))
            {
                Model.Customers_Currently_Leaving.Remove(customer);
            }
        }

        private IEnumerator Service()
        {
            while (isActiveAndEnabled && IsValid)
            {
                Controller.ConsumeCustomersFromQueue();
                on_TakenInNewCustomers?.Invoke(this,EventArgs.Empty);
                yield return new WaitForSecondsRealtime(Model.SpeedOfServiceUpgrade.SpeedOfService);
                //TODO: call Output event on model
                Controller.release_Customers();
            }
        }
        
        // private void MoveCustomers()
        // {
        //     var Bodies = Model.CurrentQueue.ToArray();
        //     Model.CurrentQueue.Clear();
        //     for (int i = 0; i < Bodies.Length; i++)
        //     {
        //         Bodies[i].MoveToNextWayPoint();
        //         Model.CurrentQueue.Enqueue(Bodies[i]);
        //     }
        // }

        private bool InitQueue()
        {
            if (QueuePath.CurveCount > GameSettings.Max_Prestige)
            {
                Debug.LogWarning($"The queue path curve line should not exceed the max prestige {GameSettings.Max_Prestige}\n" +
                                 $"aborting initialization of queue {this.gameObject.name}");
                return false;
            }

            if (QueuePath == null)
            {
                Debug.LogWarning($"No QueuePath has been Assigned\n" +
                                 $"aborting initialization of queue {this.gameObject.name}");
                return false;
            }

            if (ExitPath == null)
            {
                Debug.LogWarning($"No ExitPath has been Assigned\n" +
                                 $"aborting initialization of queue {this.gameObject.name}");
                return false;
            }
            if (QueueBarrier == null)
            {
                Debug.LogWarning($"No transform has been assigned to the Queue Barrier so Visuals Can not be represented\n" +
                                 $"and queue collisions may not work correctly\n" +
                                 $"aborting initialization of queue {this.gameObject.name}");
                return false;
            }

            Model.WorldPos_buildingExitPoint = ExitPath.GetControlPoint(0);
            QueueBarrier.gameObject.SetActive(true);
            MoveQueueBarrier();
            return true;
        }

        private void MoveQueueBarrier()
        {
            Vector2 prestiageQueueStartPoint = Model.Prestige == 0 ? QueuePath.GetControlPoint(0) : 
                QueuePath.GetControlPoint((Model.Prestige * 3) - 3); //Get the QueueStartPoint Based on the prestiage of the Building e.g prestiage 1 (starts at 0) will be the start of the 2nd bezier curve on the spline.
            Vector2 prestiageQueueEndPoint = Model.Prestige == 0
                ? QueuePath.GetControlPoint(3)
                : QueuePath.GetControlPoint(Model.Prestige * 3);//Get the QueueEndPoint Based on the prestiage of the Building e.g prestiage 1 (starts at 0) will be the End of the 2nd bezier curve on the spline.
            
            
             t = MathUtils.Normalize(0.0f, (float) GameSettings.Max_QueueUpgradeLevel,
                (float) Model.LengthOfQueueUpgrade.UpgradeLevel); //Get the Normalized Value of the current Upgrade level e.g Could be 4 Between the values of 0 and 5 (Max_QueueUpgradeLevel is a const with value 5)

            Vector2 queueDirection = QueuePath.GetLinearDirection(prestiageQueueStartPoint, prestiageQueueEndPoint, t);
            
            StartOfLineSegment = transform.TransformPoint(prestiageQueueStartPoint);
            EndOfLineSegment = transform.TransformPoint(prestiageQueueEndPoint);
            
            m_queueEndPoint = MathUtils.LinerInterpolation(StartOfLineSegment, EndOfLineSegment, t);
            //Find the Vector lying in between prestiageQueueStartPoint and prestiageQueueEndPoint with the percentage of t and assign that as the queue's current endpoint
            QueueBarrier.position = m_queueEndPoint;
            QueueBarrier.rotation = Quaternion.Euler(queueDirection);
        }

        private void Update()
        {
            foreach (IQueue customer in Model.CurrentQueue.Where(x => !x.CollidedInQueue && x.GameObject.activeSelf))
            {
                customer.ProgressThroughQueue += Time.deltaTime / Queue_Duration;
                if (customer.ProgressThroughQueue > 1f)
                    customer.ProgressThroughQueue = 1f;

                // customer.ProgressThroughQueue = MathUtils.Normalize(0.0f, t, customer.ProgressThroughQueue);

                customer.GameObject.transform.position = QueuePath.GetPoint(customer.ProgressThroughQueue);
            }

            foreach (IQueue customer in Model.Customers_Currently_Leaving)
            {
                customer.ProgressThroughQueue += Time.deltaTime / Queue_Duration;
                if (customer.ProgressThroughQueue > 1f)
                    customer.ProgressThroughQueue = 1f;

                customer.GameObject.transform.position = ExitPath.GetPoint(customer.ProgressThroughQueue);
            }
        }
    }
}