using System;
using DataStructures;
using DG.Tweening;
using UnityEngine;

namespace RCR.Settings.AI
{
    public class CustomerBrain : DynamicArtificalIntelligentBrain, IQueue
    {

        private Rigidbody2D rb;
        private Collider2D m_collider2D;
        private float RayDistance;
        public BezierSpline QueuePath { get; set; }
        public float Duration { get; set; }
        public bool StopPathProgression { get; set; }
        public bool EnteredQueue { get; set; }
        public bool LeftQueue { get; set; }
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
            RayDistance = m_collider2D.bounds.extents.x > m_collider2D.bounds.extents.y
                ? m_collider2D.bounds.extents.x
                : m_collider2D.bounds.extents.y;
        }

        public void MoveThroughQueue()
        {
            CheckForQueueBarriers();
            if(StopPathProgression)
                return;
            
            ProgressThroughQueue += Time.deltaTime / Duration;
            if (ProgressThroughQueue > 1f)
                ProgressThroughQueue = 1f;

            Vector2 position = QueuePath.GetPoint(ProgressThroughQueue);
            transform.localPosition = position;
            transform.LookAt(position + QueuePath.GetDirection(ProgressThroughQueue));
        }

        private void CheckForQueueBarriers()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward,
                RayDistance);
            if (hit.collider != null)
            {
                StopPathProgression = true;
            }
            else
            {
                StopPathProgression = false;
            }
        }

        public void On_QueueEntered(BezierSpline queue, float speed_of_queue)
        {
            QueuePath = queue;
            Duration = speed_of_queue;
        }

        public void on_QueueBusy()
        {
            transform.position = startpos;
            ProgressThroughQueue = 0.0f;
            CollidedInQueue = false;
            EnteredQueue = false;
        }
        

        public void on_QueueCompleted()
        {
            transform.position = startpos;
            ProgressThroughQueue = 0.0f;
            CollidedInQueue = false;
            EnteredQueue = false;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (EnteredQueue)
            {
                CollidedInQueue = true;
            }
            else
            {
                CollidedInQueue = false;
            }
        }
    }
}