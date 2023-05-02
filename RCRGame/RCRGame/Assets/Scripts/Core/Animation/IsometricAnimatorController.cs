using System;
using System.Collections.Generic;
using Core3.MonoBehaviors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Animation
{
    [RequireComponent(typeof(Animator))]
    public class IsometricAnimatorController : BaseMonoBehavior
    {
        protected Animator Animator;
        
        [Title("Configurations", TitleAlignment = TitleAlignments.Centered)] 
        [Required] [SerializeField] private RuntimeAnimatorController _runtimeAnimatorController;
        public readonly Dictionary<IsometricDirection, string> Static_Directions_Animations =
            new Dictionary<IsometricDirection, string>()
            {
                {IsometricDirection.North, "Static N"},
                {IsometricDirection.NorthWest, "Static NW"},
                {IsometricDirection.West, "Static W"},
                {IsometricDirection.SouthWest, "Static SW"},
                {IsometricDirection.South, "Static S"},
                {IsometricDirection.SouthEast, "Static SE"},
                {IsometricDirection.East, "Static E"},
                {IsometricDirection.NorthEast, "Static NE"},
            };

        private IsometricDirection _direction;
        private Vector3 _currentPosition;

        public IsometricDirection Direction
        {
            get => _direction;
            protected set
            {
                if(value == _direction)
                    return;
                _direction = value;
                Animator.Play(Static_Directions_Animations[value]);
            }
        }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Animator.runtimeAnimatorController = _runtimeAnimatorController;
            _currentPosition = transform.position;
        }

        private void Update()
        {
            Vector2 direction2D = (_currentPosition - transform.position).normalized * -1;
            float step = 360 / 8;
            float offset = step / 2f;
            float angle = Vector2.SignedAngle(Vector2.up, direction2D);

            angle += offset;
            if (angle < 0)
                angle += 360;
            float stepCount = angle / step;
            Direction = (IsometricDirection) Mathf.FloorToInt(stepCount);
            _currentPosition = transform.position;
        }
    }
}