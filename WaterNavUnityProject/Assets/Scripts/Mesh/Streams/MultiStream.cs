﻿using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mesh.Streams
{
    public struct MultiStream: IMeshStreams
    {
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float3> stream0, stream1; //Position, Normals

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float4> stream2; //Tangent
        
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float2> stream3; //UV Coords
        
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<TriangleUInt16> triangles;

        public void Setup(UnityEngine.Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount)
        {
            var descriptor =
                new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

            descriptor[0] = new VertexAttributeDescriptor(dimension: 3); //Postion
            descriptor[1] = new VertexAttributeDescriptor(VertexAttribute.Normal,
                dimension: 3, stream: 1); //Normal

            descriptor[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent,
                dimension: 4, stream: 2); //Tangent

            descriptor[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0,
                dimension: 2, stream: 3);
            
            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();
            
            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16);

            meshData.subMeshCount = 1;
            meshData.SetSubMesh(
                0, new SubMeshDescriptor(0, indexCount)
                {
                    bounds = bounds,
                    vertexCount = vertexCount
                },
                MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices);

            stream0 = meshData.GetVertexData<float3>();
            stream1 = meshData.GetVertexData<float3>(1);
            stream2 = meshData.GetVertexData<float4>(2);
            stream3 = meshData.GetVertexData<float2>(3);
            triangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex vertex)
        {
            stream0[index] = vertex.position;
            stream1[index] = vertex.normal;
            stream2[index] = vertex.tangent;
            stream3[index] = vertex.texCoord0;

        }

        public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;

    }
}