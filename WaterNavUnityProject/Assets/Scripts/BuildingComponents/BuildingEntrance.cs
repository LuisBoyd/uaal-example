using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures;
using RCR.Settings.AI;
using UnityEngine;

namespace BuildingComponents
{
    public class BuildingEntrance : MonoBehaviour
    {
        public static float PushForce = 0.5f;
        public static float TimeToFail = 10.5f;
        private BuildingView m_buildingView;
        private BoxCollider2D m_boxCollider2D;
        private BezierSpline EntrancePath;
        private bool NoAccessToQueue = false;
        private Dictionary<Collider2D, IQueue> m_lookup;
        private List<Collider2D> m_toRemove;

        private void Awake()
        {
            m_buildingView = GetComponentInParent<BuildingView>();
            m_lookup = new Dictionary<Collider2D,IQueue>();
            m_boxCollider2D = GetComponent<BoxCollider2D>();
            m_toRemove = new List<Collider2D>();
            EntrancePath = GetComponent<BezierSpline>();
            m_buildingView.on_TakenInNewCustomers += BuildingViewOnon_TakenInNewCustomers;
            m_buildingView.on_endOfQueueOccupied += BuildingViewOnon_endOfQueueOccupied;
        }
        

        private void OnDestroy()
        {
            m_buildingView.on_TakenInNewCustomers -= BuildingViewOnon_TakenInNewCustomers;
            m_buildingView.on_endOfQueueOccupied -= BuildingViewOnon_endOfQueueOccupied;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<IQueue>(out IQueue customer))
            {
                m_lookup.Add(col, customer);
                customer.LeftQueue = false;
                customer.EnteredQueue = false;
                StartCoroutine(QueueEntranceTimer(col));
            }
        }

        private void Update()
        {
            foreach (System.Collections.Generic.KeyValuePair<Collider2D, IQueue> customer in m_lookup)
            {
                if(customer.Value.CollidedInQueue)
                    continue;
                
                customer.Value.ProgressThroughQueue += Time.deltaTime / PushForce;
                if (customer.Value.ProgressThroughQueue >= 1f)
                {
                    on_QueueCompleted(customer.Key);
                    m_toRemove.Add(customer.Key);
                    continue;
                }
                if (customer.Value.ProgressThroughQueue > 1f)
                {
                    customer.Value.ProgressThroughQueue = 1f;
                }

                if (NoAccessToQueue && customer.Value.ProgressThroughQueue >= 0.80f)
                {
                    customer.Value.ProgressThroughQueue = 0.80f;
                }

                customer.Value.GameObject.transform.position =
                    EntrancePath.GetPoint(customer.Value.ProgressThroughQueue);
            }
            foreach (Collider2D col in m_toRemove)
            {
                m_lookup.Remove(col);
            }
        }

        private void on_QueueCompleted(Collider2D col)
        {
            if (m_lookup.ContainsKey(col))
            {
                m_lookup[col].ProgressThroughQueue = 0.0f;
                m_lookup[col].EnteredQueue = true;
                //m_buildingView.AddCustomerToQueue(m_lookup[col]);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (m_lookup.ContainsKey(col))
            {
                //m_lookup[col].ProgressThroughQueue = 0.0f;
                if (m_lookup[col].LeftQueue)
                {
                    m_lookup.Remove(col);
                    return;
                }
            }
        }

        private IEnumerator QueueEntranceTimer(Collider2D collider2D)
        {
            yield return new WaitForSecondsRealtime(TimeToFail);
            if (m_lookup.ContainsKey(collider2D))
            {
                if (!m_lookup[collider2D].EnteredQueue)
                {
                    //m_lookup[collider2D].on_QueueBusy();
                    m_lookup.Remove(collider2D);
                }
            }
        }

        private IEnumerator WaitBeforeSendingIn()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            foreach (System.Collections.Generic.KeyValuePair<Collider2D, IQueue> customer in m_lookup)
            {
                customer.Value.CollidedInQueue = false;
            }
        }
        
        private void BuildingViewOnon_TakenInNewCustomers(object sender, EventArgs e)
        {
            StartCoroutine(WaitBeforeSendingIn());
            NoAccessToQueue = false;
        }
        private void BuildingViewOnon_endOfQueueOccupied(object sender, EventArgs e)
        {
            NoAccessToQueue = true;
        }

        [Obsolete]
        private Vector3 GetForcePoint(Collider2D col)
        {
            Bounds boundingBox = m_boxCollider2D.bounds;
            return transform.TransformPoint(m_buildingView.ApplyEntranceForceRight
                ? new Vector3(boundingBox.center.x + boundingBox.extents.x + col.bounds.extents.x, boundingBox.center.y)
                : new Vector3(boundingBox.center.x + (-boundingBox.extents.x) + (-col.bounds.extents.x),
                    boundingBox.center.y));
        }
        
    }
}