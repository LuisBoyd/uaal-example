using System;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(AISteering))]
    public class DynamicAiAgent : AiAgent
    {
        public AISteering steering { get; private set; }

        private void Awake()
        {
            steering = GetComponent<AISteering>();
        }
    }
}