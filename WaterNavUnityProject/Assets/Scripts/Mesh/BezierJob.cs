using PathCreation;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Mesh
{
    // [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct BezierJob <G,S> : IJobFor
    where G : struct, IBezierGenerator
    where S : struct, IMeshStreams
    {
        private G generator;

        [WriteOnly] 
        private S streams;

        public void Execute(int index) => generator.Execute(index, streams);

        public static JobHandle ScheduleParallel(
           UnityEngine.Mesh mesh, UnityEngine.Mesh.MeshData meshData, VertexPath path, JobHandle dependency)
        {
            var job = new BezierJob<G, S>();
            job.generator.LocalPositions = new NativeArray<Vector3>(path.localPoints , Allocator.TempJob);
            job.generator.LocalTangents = new NativeArray<Vector3>(path.localTangents , Allocator.TempJob);
            job.generator.LocalNormals = new NativeArray<Vector3>(path.localNormals, Allocator.TempJob);
            job.generator.Bounds = path.bounds;
            job.streams.Setup(
                meshData,
                mesh.bounds = job.generator.Bounds,
                job.generator.VertexCount,
                job.generator.IndexCount);
            
            
            return job.ScheduleParallel(
                job.generator.JobLength, 1, dependency);
        }
    }
    public delegate JobHandle BezierJobScheduleDelegate(UnityEngine.Mesh mesh, UnityEngine.Mesh.MeshData meshData,
        VertexPath path, JobHandle dependency);
}