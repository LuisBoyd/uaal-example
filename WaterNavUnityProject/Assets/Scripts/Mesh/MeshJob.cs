using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Mesh
{
    //[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshJob <G,S> : IJobFor
    where G : struct, IMeshGenerator
    where S : struct, IMeshStreams
    {
        private G generator;

        [WriteOnly] 
        private S streams;

        public void Execute(int index) => generator.Execute(index, streams);

        public static JobHandle ScheduleParallel(
           UnityEngine.Mesh mesh, UnityEngine.Mesh.MeshData meshData, int resolution, JobHandle dependency)
        {
            var job = new MeshJob<G, S>();
            job.generator.Resolution = resolution;
            job.streams.Setup(
                meshData,
                mesh.bounds = job.generator.Bounds,
                job.generator.VertexCount,
                job.generator.IndexCount);

            return job.ScheduleParallel(
                job.generator.JobLength, 1, dependency);
        }
    }
    public delegate JobHandle MeshJobScheduleDelegate(UnityEngine.Mesh mesh, UnityEngine.Mesh.MeshData meshData,
        int resolution, JobHandle dependency);
}