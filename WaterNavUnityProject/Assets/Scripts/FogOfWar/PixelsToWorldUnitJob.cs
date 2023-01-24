using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace FogOfWar
{
    public struct PixelsToWorldUnitJob: IJobParallelFor
    {
        [ReadOnly]
        public float PixelsPerUnit;

        public NativeArray<Vector2> results;

        public void Execute(int index)
        {
            results[index] = ConvertToWorldUnit(index);
        }

        private Vector2 ConvertToWorldUnit(int index)
        {
            return Get2DPosition(index) / PixelsPerUnit;
            //Vector2 Pos / PixelPerUnit
        }

        private float2 Get2DPosition(int index)
        {
            return float2(floor(index % results.Length), floor((float)index / results.Length));
        }
    }
}