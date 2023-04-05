using System;
using UnityEngine;
using Utilities;
using WQS;

namespace WQS
{
    public abstract class WorldObject : MonoBehaviour, IWQSDiscover
    {
#if UNITY_EDITOR
        [SerializeField] 
        protected Color DebugSphereColor;  
#endif
        public bool CurrentlySelected { get; set; }

        public Collider2D Collider2D
        {
            get => GetComponent<Collider2D>();
        }

        public GameObject GameObject
        {
            get => gameObject;
        }

        public float DistanceTo(Transform agent)
        {
            return Vector2.Distance(transform.position, agent.position);
        }

        public float Dot(Transform agent)
        {
            return Vector2.Dot(transform.position, agent.position);
        }

        public bool PathFinding(Transform agent, PathFindingMode mode, out Vector3[] resultingPath)
        {
            resultingPath = PathEngine.Instance.FindPath(agent.position, transform.position, PathFindingMode.Land).ToArray();
            if (resultingPath.Length == 0)
            {
                resultingPath = null;
                return false;
            }
            return true;
        }

        public bool Trace(Transform agent, out RaycastHit2D hit2D)
        {
            hit2D = Physics2D.Raycast(agent.transform.position,
                (agent.transform.position - transform.position).normalized);
            if (hit2D.collider != Collider2D)
                return false;
            return true;
        }

        public bool Overlap(Transform agent, ShapeInfo info, out Collider2D[] collider2Ds)
        {
            switch (info.shape)
            {
                case OverlapShape.Square:
                    collider2Ds = Physics2D.OverlapBoxAll(info.origin, info.size, info.angle, info.layerMask);
                    break;
                case OverlapShape.Circle:
                    collider2Ds = Physics2D.OverlapCircleAll(info.origin, info.radius, info.layerMask);
                    break;
                case OverlapShape.Capsule:
                    collider2Ds = Physics2D.OverlapCircleAll(info.origin, info.radius, info.layerMask);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(info.shape), info.shape, null);
            }

            if (collider2Ds != null)
            {
                if (collider2Ds.Length > 0)
                    return true;
            }
            return false;
        }

        public bool Tag(string tag, out GameObject obj)
        {
            obj = this.gameObject;
            return obj.tag.Equals(tag);
        }
#if UNITY_EDITOR

        public void DebugDiscover()
        {
            Gizmos.color = DebugSphereColor;
            DebugDrawer.DrawCircle(transform.position, DebugSettings.WQSsphereRadius, DebugSettings.WQSsphereSegments,
                Gizmos.color,DebugSettings.WQSsphereDebugtime);
        }
        private void OnDrawGizmos()
        {
            if(!CurrentlySelected)
                return;
            DebugDiscover();
        }

#endif
    }
}