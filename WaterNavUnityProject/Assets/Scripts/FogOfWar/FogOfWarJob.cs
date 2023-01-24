using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace RCR.Settings.FogOfWar
{
    public struct FogOfWarJob : IJobParallelFor
    {
        [ReadOnly] 
        public NativeArray<int> StateOfPosition;

        public NativeArray<RGBA32> FogOfWarData;
        

        public void Execute(int index)
        {
            FogOfWarData[index] = StateOfPosition[index] % 2 == 0 ? RGBA32.White : RGBA32.Zero;
            //FogOfWarData[index] = RGBA32.White;
        }
    }
}