using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEngine;
using float2 = Unity.Mathematics.float2;

namespace RCR.Settings.FogOfWar
{
    /// <summary>
    /// Records number of times a ray or line intersect with a collection of rays/lines
    /// Only supports 2D at the moment
    /// </summary>
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct IntersectionNonAllocJob : IJob
    {
        [ReadOnly]
        public NativeArray<Vector3> VertexPositions;
        
        //list of Origins and directions
        
        [ReadOnly]
        public NativeArray<Vector2> Origins;
        
        public NativeArray<int> result;

        public void Execute()
        {
            for (int i = 0; i < Origins.Length; i++)
            {
                Vector2 origin = Origins[LoopIndex(i, Origins.Length)];
                for (int j = 0; j < VertexPositions.Length; j++)
                {
                    Vector2 q = VertexPositions[LoopIndex(j, VertexPositions.Length)];
                    Vector2 s = VertexPositions[LoopIndex(j + 1, VertexPositions.Length)];
                    Vector2 direction = GetMidPoint(q, s);
                    result[i] += LineIntersect(q, s, origin, direction)? 1 : 0;
                }
            }
        }

        private Vector2 GetMidPoint(Vector2 a, Vector2 b) => (a - b) / 2;
       

        private int LoopIndex(int i, int length)
        {
            return (i + length) % length;
        }

        private bool LineIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float2 cmp = float2(c.x - a.x, c.y - a.y);
            float2 r = float2(b.x - a.x, b.y - a.y); //  r is the end point of
            float2 s = float2(d.x - c.x, d.y - c.y); // s end point

            float cmPxr = cmp.x * r.y - cmp.y * r.x;
            float cmPxs = cmp.x * s.y - cmp.y * s.x;
            float rxs = r.x * s.y - r.y * s.x;

            if (cmPxr == 0f)
            {
                //Lines are collinear and so intersect if the overlap
                return ((c.x - a.x < 0f) != (c.x - b.x < 0f)) ||
                       ((c.y - a.y < 0f) != (c.y - b.y < 0f));
            }

            if (rxs == 0f)
                return false;  //Lines are parallel and do not intersect

            float rxsr = 1f / rxs;
            float t = cmPxs * rxsr;
            float u = cmPxr * rxsr;

            return (t >= 0f) && (t <= 1f) && (u >= 0f) && (u <= 1f);
        }
    }
}