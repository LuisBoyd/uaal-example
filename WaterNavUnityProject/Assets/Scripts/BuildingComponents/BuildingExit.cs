using System;
using System.Collections.Generic;
using DataStructures;
using RCR.Settings.AI;
using UnityEngine;

namespace BuildingComponents
{
    public class BuildingExit : MonoBehaviour
    {
        public static float PushForce = 0.5f;
        private BuildingView m_buildingView;
        private BoxCollider2D m_boxCollider2D;
        private Dictionary<Collider2D, IQueue> m_lookup;
        private BezierSpline Exitpath;

        private void Awake()
        {
            m_buildingView = GetComponentInParent<BuildingView>();
            m_boxCollider2D = GetComponent<BoxCollider2D>();
            Exitpath = GetComponent<BezierSpline>();
            m_lookup = new Dictionary<Collider2D, IQueue>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<IQueue>(out IQueue customer))
            {
                //m_buildingView.RemoveCustomerFromBuilding(customer);
                m_lookup.Add(col, customer);
            }
        }

        private void Update()
        {
            foreach (System.Collections.Generic.KeyValuePair<Collider2D, IQueue> customer in m_lookup)
            {
                customer.Value.ProgressThroughQueue += Time.deltaTime / PushForce;
                if (customer.Value.ProgressThroughQueue > 1f)
                    customer.Value.ProgressThroughQueue = 1f;

                customer.Value.GameObject.transform.position = Exitpath.GetPoint(customer.Value.ProgressThroughQueue);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (m_lookup.ContainsKey(other))
            {
                //m_lookup[other].on_QueueCompleted();
                m_lookup.Remove(other);
            }
        }

        [Obsolete]
        private Vector3 GetForcePoint(Collider2D col)
        {
            Bounds boundingBox = m_boxCollider2D.bounds;
            return transform.TransformPoint(m_buildingView.ApplyEntranceForceRight
                ? new Vector3(boundingBox.center.x + (-boundingBox.extents.x) + (-col.bounds.extents.x),
                    boundingBox.center.y)
                : new Vector3(boundingBox.center.x + boundingBox.extents.x + col.bounds.extents.x,
                    boundingBox.center.y));
        }
    }
}