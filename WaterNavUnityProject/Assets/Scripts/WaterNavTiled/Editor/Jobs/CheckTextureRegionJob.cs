using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace WaterNavTiled.Editor.Jobs
{
    [Obsolete("This Class is obselete passing in NativeArrayColor32 seems to be " +
              "passing in smaller amount of data")]
    public struct CheckTextureRegionJob: IJob
    {
        public int xpos;
        public int ypos;
        public int width;
        public int height;
        [ReadOnly] public NativeArray<Color32> TextureData;
        public NativeArray<int> result;

        public void Execute()
        {
            for (int x = xpos; x < width + xpos; x++)
            {
                for (int y = ypos; y < height + ypos; y++)
                {
                    result[width * x + y] = TextureData[width * x + y].a != 0 ? 1 : 0;
                }
            }
        }
    }
}