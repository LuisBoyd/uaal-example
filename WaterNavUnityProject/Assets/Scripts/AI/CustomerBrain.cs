using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace RCR.Settings.AI
{
    public class CustomerBrain : DynamicArtificalIntelligentBrain, IQueue
    {
        [SerializeField] 
        private Transform RayCastHelper;

        public bool TestMove;
        private float x;
        private Rigidbody2D rb;
        private Collider2D m_collider2D;
        private float RayDistance;
        public BezierSpline QueuePath { get; set; }
        public Action on_CompletedCallback { get; set; }
        public float Duration { get; set; }
        public float TimeInQueue { get; set; }
        public bool StopPathProgression { get; set; }
        public bool EnteredQueue { get; set; }
        public bool LeftQueue { get; set; }
        public bool HasBeenServiced_LeavingQueue { get; set; }
        public event EventHandler<IQueue> on_LeftQueue;

        public GameObject GameObject
        {
            get => this.gameObject;
        }

        public float ProgressThroughQueue { get; set; }

        public bool CollidedInQueue
        {
            get
            {
                return m_collidedInQueue;
            }
            set
            {
                if (value)
                {
                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0.0f;
                }

                m_collidedInQueue = value;
            }
        }

        private bool m_collidedInQueue = false;
        public int QueueMaxLength { get; set; }
        public int QueuePosition { get; set; }

        public Vector3 startpos;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            m_collider2D = GetComponent<Collider2D>();
            startpos = transform.position;
            x = transform.position.x;
        }

        private void Update()
        {
            if (TestMove && QueuePath == null)
            {
                x += Time.deltaTime / 1f;
                transform.position = new Vector3(x, transform.position.y);
            }
               
        }

        public void MoveThroughQueue()
        {
            if (!HasBeenServiced_LeavingQueue)
            {
                CheckForTimeInQueue();
                CheckForQueueBarriers();
                if (StopPathProgression)
                    return;
            }

            ProgressThroughQueue += Time.deltaTime / Duration;
            if (ProgressThroughQueue > 1f)
            {
                ProgressThroughQueue = 1f;
            }
            Vector3 position = QueuePath.GetPoint(ProgressThroughQueue);
            float angle = Vector2.SignedAngle(Vector2.right, (position - transform.position));
            transform.localPosition = position;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        

      

        private void CheckForQueueBarriers()
        {
            int layerMask = 1 << 6;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right,
                m_collider2D.bounds.extents.x, layerMask);
            if (hit.collider != null)
            {
                StopPathProgression = true;
            }
            else
            {
                StopPathProgression = false;
            }
        }

        private void CheckForTimeInQueue()
        {
            //TODO implement checking the time a customer is in the queue to then boot them out of it if they have spent to much time
        }

        public void On_QueueEntered(BezierSpline queue, float speed_of_queue)
        {
            ResetValues();
            QueuePath = queue;
            Duration = speed_of_queue;
            rb.isKinematic = true;
            //Turn off A* Movement flag
        }

        private IEnumerator leaveQueue_Serviced()
        {
            while (ProgressThroughQueue < 1f)
            {
                ProgressThroughQueue += Time.deltaTime / Duration;
                Vector3 position = QueuePath.GetPoint(ProgressThroughQueue);
                float angle = Vector2.SignedAngle(Vector2.right, (position - transform.position));
                transform.localPosition = position;
                transform.eulerAngles = new Vector3(0, 0, angle);
                yield return null;
            }
            LeaveQueue();
        }

        public void LeaveQueue_Serviced()
        {
            StartCoroutine(leaveQueue_Serviced());
        }

        public void on_QueueBusy()
        {
            transform.position = startpos;
            ResetValues();
        }

        public void LeaveQueue()
        {
            rb.isKinematic = false;
            transform.position = startpos;
            TestMove = false;
            ResetValues();
        }

        private void ResetValues()
        {
            ProgressThroughQueue = 0.0f;
            TimeInQueue = 0.0f;
            StopPathProgression = false;
            QueuePath = null;
            Duration = 0.0f;
            LeftQueue = false;
            on_CompletedCallback = null;
        }
    }
}