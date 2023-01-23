using PathCreation;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;

namespace Mesh.Generators
{
    public struct UVBezier : IBezierGenerator
    {
        public Bounds Bounds { get; set; }


        public int VertexCount => (ResolutionU + 1) * (ResolutionV + 1) - 2;

        public int IndexCount => 6 * ResolutionU * (ResolutionV - 1);

        public int JobLength => LocalPositions.Length;
        
        public NativeArray<Vector3> LocalPositions { get; set; }
        public NativeArray<Vector3> LocalNormals { get; set; }
        public NativeArray<Vector3> LocalTangents { get; set; }

        private int ResolutionU => 4 * LocalPositions.Length;
        private int ResolutionV => 2 * LocalPositions.Length;
        
        private int localPositionLength
        {
            get => LocalPositions.Length;
        }
        
        private int localNormalLength
        {
            get => LocalNormals.Length;
        }
        
        private int localTangentLength
        {
            get => LocalTangents.Length;
        }


        private int LoopIndex(int i, int length)
        {
            return (i + length) % length;
        }
        //131
        public void Execute<S>(int i, S streams) where S : struct, IMeshStreams
        {
            int vi = LoopIndex(i, localPositionLength);
            var vertex = new Vertex();
            vertex.position = LocalPositions[LoopIndex(i, localPositionLength)];
            vertex.normal = LocalNormals[LoopIndex(i, localNormalLength)];
            vertex.tangent.xyz = LocalTangents[LoopIndex(i, localTangentLength)];
            streams.SetVertex(LoopIndex(i, localPositionLength), vertex);
            
            streams.SetTriangle(LoopIndex(i, localPositionLength) , vi + int3(-vi, -2, -1));
        }
    }
}