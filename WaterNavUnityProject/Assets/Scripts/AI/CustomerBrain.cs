using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataStructures;
using DG.Tweening;
using Events.Library.Models;
using Events.Library.Models.TestEvents;
using NewManagers;
using Unity.Mathematics;
using UnityEngine;

namespace RCR.Settings.AI
{
    public class CustomerBrain : DynamicArtificalIntelligentBrain//, IQueue
    {
        #region TestCode
        public bool TestMove;
        private float x;
        #endregion

        private Rigidbody2D rb;
        private Collider2D m_collider2D;
        //public BezierSpline QueuePath { get; set; }
        public float Duration { get; set; }
        public float TimeInQueue { get; set; }
        public bool StopPathProgression { get; set; }
        public bool EnteredQueue { get; set; }
        public bool LeftQueue { get; set; }
        public float ProgressThroughQueue { get; set; }

        public Vector3 startpos;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            m_collider2D = GetComponent<Collider2D>();
            #region debugCode

            GameManager_2_0.Instance.EventBus.Subscribe<UnityEditorClickedBool>(On_UnityClickedEditorBoolEvent);
            startpos = transform.position;
            x = transform.position.x;
            #endregion
            
        }

        private void Update()
        {
            // #region DebugCode
            //
            // if (TestMove && QueuePath == null)
            // {
            //     x += Time.deltaTime / 1f;
            //     transform.position = new Vector3(x, transform.position.y);
            // }
            //
            // #endregion
            
        }

        public void MoveThroughQueue()
        {
            CheckForTimeInQueue();
            CheckForQueueBarriers();
            if (StopPathProgression)
                return;
            ProgressThroughQueue += Time.deltaTime / Duration;
            if (ProgressThroughQueue > 1f)
            {
                ProgressThroughQueue = 1f;
            }
            //Vector3 position = QueuePath.GetPoint(ProgressThroughQueue);
            // float angle = Vector2.SignedAngle(Vector2.right, (position - transform.position));
            // transform.localPosition = position;
            //transform.eulerAngles = new Vector3(0, 0, angle);
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

        public void On_QueueEntered(float speed_of_queue)
        {
            // ResetValues();
            // QueuePath = queue;
            // Duration = speed_of_queue;
            // rb.isKinematic = true;
            // //Turn off A* Movement flag
        }

        private IEnumerator leaveQueue_Serviced()
        {
            while (ProgressThroughQueue < 1f)
            {
                // ProgressThroughQueue += Time.deltaTime / Duration;
                // Vector3 position = QueuePath.GetPoint(ProgressThroughQueue);
                // float angle = Vector2.SignedAngle(Vector2.right, (position - transform.position));
                // transform.localPosition = position;
                // transform.eulerAngles = new Vector3(0, 0, angle);
                // yield return null;
            }
            LeaveQueue();
            yield return null;
        }

        public void LeaveQueue_Serviced(Vector3 LeavingPoint)
        {
            transform.position = LeavingPoint;
            gameObject.SetActive(true);
            StartCoroutine(leaveQueue_Serviced());
        }
        

        public void LeaveQueue()
        {
            rb.isKinematic = false;
            LeftQueue = true;
            transform.position = startpos;
            TestMove = false;
        }

        public void EnterService()
        {
            gameObject.SetActive(false);
            ResetValues();
        }

        public void LeaveService(Vector3 LeavingPoint)
        { 
            transform.position = LeavingPoint;
            gameObject.SetActive(true);
        }

        private void ResetValues()
        {
            // ProgressThroughQueue = 0.0f;
            // TimeInQueue = 0.0f;
            // StopPathProgression = false;
            // QueuePath = null;
            // Duration = 0.0f;
            // LeftQueue = false;
        }

        private IEnumerator On_UnityClickedEditorBoolEvent(UnityEditorClickedBool evnt, EventArgs args)
        {
            Debug.Log($"{this.gameObject.name} Ran an event");
            yield return null;
        }
        // private Task On_UnityClickedEditorBoolEvent<TEvent>(TEvent arg1, EventArgs arg2) where TEvent : BaseEvent
        // {
        //     Debug.Log($"{this.gameObject.name} Ran an event");
        //     return null;
        // }
    }
}