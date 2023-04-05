using UnityEngine;

namespace WQS
{
    public struct ShapeInfo
    {
        public Vector2 size;
        public Vector2 origin;
        public float angle;
        public float radius;
        public int layerMask;
        public CapsuleDirection2D CapsuleDirection2D;
        public OverlapShape shape;
    }
}