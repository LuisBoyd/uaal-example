using System;
using System.Collections.Generic;
using UnityEngine;

namespace RCRCoreLib.Core.Animation
{
    [RequireComponent(typeof(Animator))]
    public  class IsometricAnimatorController : MonoBehaviour
    {
        protected Animator Animator;
        
        [SerializeField]
        protected IsometricAnimatorData IsometricAnimatorData;
        public Vector3 CurrentPosition;

        private readonly Dictionary<IsometricDirection, string> Static_Directions_Animations =
            new Dictionary<IsometricDirection, string>()
            {
                {IsometricDirection.Notrh, "Static N"},
                {IsometricDirection.NorthWest, "Static NW"},
                {IsometricDirection.West, "Static W"},
                {IsometricDirection.SouthWest, "Static SW"},
                {IsometricDirection.South, "Static S"},
                {IsometricDirection.SouthEast, "Static SE"},
                {IsometricDirection.East, "Static E"},
                {IsometricDirection.NorthEast, "Static NE"},

            };

        private IsometricDirection _direction;

        public IsometricDirection Direction
        {
            get => _direction;
            protected set
            {
                if (value == _direction)
                    return;

                _direction = value;
                Animator.Play(Static_Directions_Animations[value]);
            }
        }
        
        public virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Animator.runtimeAnimatorController = IsometricAnimatorData.RuntimeAnimatorController;
            CurrentPosition = transform.position;
        }

        public void SetDirection(float progress)
        {
            Vector2 Direction2D = (CurrentPosition - transform.position).normalized * -1;

            float step = 360 / 8; //45 once Circle and 8 slices/ Directions
            float offset = step / 2; //22.5
            float angle = Vector2.SignedAngle(Vector2.up, Direction2D); //Return the signed angle in degrees between A and B

            angle += offset;
            if (angle < 0)
                angle += 360;

            float stepCount = angle / step;
            Direction = (IsometricDirection) Mathf.FloorToInt(stepCount);
            CurrentPosition = transform.position;

        }
        
        
    }
}