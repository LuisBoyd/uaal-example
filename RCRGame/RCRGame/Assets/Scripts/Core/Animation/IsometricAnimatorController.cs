using System;
using UnityEngine;

namespace RCRCoreLib.Core.Animation
{
    [RequireComponent(typeof(Animator))]
    public abstract class IsometricAnimatorController : MonoBehaviour
    {
        protected Animator Animator;
        protected IsometricAnimatorData IsometricAnimatorData;

        public Vector2 _direction { get; protected set; }
        
        public virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Animator.avatar = IsometricAnimatorData.RuntimeAnimationAvatar;
            Animator.runtimeAnimatorController = IsometricAnimatorData.RuntimeAnimatorController;
        }

        public  void SetDirection(Vector3 direction)
        {
            Vector2 Direction2D = new Vector2(direction.x, direction.y).normalized;

            float step = 360 / 8; //45 once Circle and 8 slices/ Directions
            float offset = step / 2; //22.5

            float angle = Vector2.SignedAngle(Vector2.up, Direction2D); //Return the signed angle in degrees between A and B

            angle += offset;
            if (angle < 0)
                angle += 360;

            float stepCount = angle / step;
            
        }
        
        
    }
}