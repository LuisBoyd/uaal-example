﻿using Unity.Mathematics;
using UnityEngine;

namespace Mesh
{
    public struct Vertex
    {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }
}