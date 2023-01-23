using PathCreation;
using Unity.Collections;
using UnityEngine;

namespace Mesh
{
    public interface IBezierGenerator
    {
        Bounds Bounds { get; set; }
        
        int VertexCount { get;}
        
        int IndexCount { get; }
        
        int JobLength { get; }

        NativeArray<Vector3> LocalPositions { get; set; }
        NativeArray<Vector3> LocalNormals { get; set; }
        NativeArray<Vector3> LocalTangents { get; set; }

        void Execute<S>(int i, S streams) where S : struct, IMeshStreams;
    }
}