using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace WaterNavTiled.Editor.Jobs
{
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
                    result[0] = TextureData[width * x + y].a != 0 ? 1 : 0;
                }
            }
        }
    }
}