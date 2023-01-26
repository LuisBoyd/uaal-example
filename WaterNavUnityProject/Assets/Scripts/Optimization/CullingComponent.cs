using System;
using RCR.Utilities;
using UnityEngine;

namespace RCR.Settings.Optimization
{
    public abstract class CullingComponent : MonoBehaviour, ICulling
    {
        [SerializeField] 
        private float radius;
        
        public Guid ID
        {
            get => _ID;
            set => _ID = value;
        }

        private Guid _ID = Guid.NewGuid();

        protected BoundingSphere _BoundingSphere;

        public BoundingSphere BoundingSphere
        {
            get => _BoundingSphere;
            set => _BoundingSphere = value;
        }

        protected virtual void OnEnable()
        {
            _ID = CullingManager.Instance.AddCullingObject(this);
            Collider collider = GetComponent<Collider>();
            if (LBUtilities.AssertNull(collider))
            {
                radius = collider.bounds.size.x > collider.bounds.size.y
                    ? collider.bounds.size.x
                    : collider.bounds.size.y;
            }

            _BoundingSphere = new BoundingSphere()
            {
                position = transform.position,
                radius = this.radius
            };
        }
        protected virtual void OnDisable()
        {
            CullingManager.Instance.RemoveCullingObject(_ID);
        }

        public abstract void ChangeInCulling(CullingGroupEvent evnt);
        
        protected abstract void InView(CullingGroupEvent evnt);
        
        protected abstract void OutOfView(CullingGroupEvent evnt);

    }
}