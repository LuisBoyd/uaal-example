using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class SteeringDetails
    {
        [SerializeField] 
        private float speed; //Speed of the Agent.
        public float Speed
        {
            get => speed;
        }

        [SerializeField] 
        private float validationInterval = 0.1f;
        public float ValidationInterval
        {
            get => validationInterval;
        }
    }
}