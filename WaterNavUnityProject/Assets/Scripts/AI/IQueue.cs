using System;
using DataStructures;
using UnityEngine;

namespace RCR.Settings.AI
{
    public interface IQueue
    {

        public BezierSpline QueuePath { get; set; }
        public float Duration { get; set; }
        public float TimeInQueue { get; set; }
        public bool StopPathProgression { get; set; }
        public bool LeftQueue { get; set; }
        public float ProgressThroughQueue { get; set; }
        public void MoveThroughQueue();
        public void On_QueueEntered(BezierSpline queue, float speed_of_queue);
        public void LeaveQueue_Serviced(Vector3 LeavingPoint);
        public void LeaveQueue();
        public void EnterService();

    }
}