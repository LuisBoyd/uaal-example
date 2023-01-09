using DataStructures;
using UnityEngine;

namespace RCR.Settings.AI
{
    public interface IQueue
    {

        public BezierSpline QueuePath { get; set; }
        public float Duration { get; set; }
        
        public bool StopPathProgression { get; set; }
        
        public bool EnteredQueue { get; set; }
        public bool LeftQueue { get; set; }
        
        public GameObject GameObject { get; }
        public float ProgressThroughQueue { get; set; }
        
        public bool CollidedInQueue { get; set; }

        public void MoveThroughQueue();

        public void On_QueueEntered(BezierSpline queue, float speed_of_queue);

        public void on_QueueBusy();
        
        public void on_QueueCompleted();
    }
}