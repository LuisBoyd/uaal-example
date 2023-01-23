using System;
using Mesh.Generators;
using Mesh.Streams;
using PathCreation;
using UnityEngine;

namespace Mesh
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    [RequireComponent(typeof(PathCreator))]
    public class ProceduralBezier : MonoBehaviour
    {
        private static BezierJobScheduleDelegate[] jobs =
        {
            BezierJob< UVBezier, SingleStream>.ScheduleParallel
        };

        public enum MeshType
        {
            Bezier
        };

        [SerializeField] 
        private MeshType meshType;

        private PathCreator m_pathCreator;
        
        [System.Flags]
        public enum GizmoMode
        {
            Nothing = 0,
            Vertices = 1,
            Normals = 0b10,
            Tangents = 0b100
        }

        [SerializeField] 
        private GizmoMode gizmos;
        
        public enum MaterialMode
        {
            Flat,
            Ripple,
            LatLonMap,
            CubeMap
        }

        [SerializeField] 
        private MaterialMode material;

        [SerializeField]
        private Material[] materials;

        private UnityEngine.Mesh mesh;

        private MeshRenderer Renderer;
        private MeshCollider MeshCollider;

        private Vector3[] vertices, normals;

        private Vector4[] tangents;

        protected virtual void Awake()
        {
            mesh = new UnityEngine.Mesh
            {
                name = "Procedural Mesh"
            };
            GetComponent<MeshFilter>().mesh = mesh;
            Renderer = GetComponent<MeshRenderer>();
            MeshCollider = GetComponent<MeshCollider>();
            m_pathCreator = GetComponent<PathCreator>();

        }

        protected void OnDrawGizmos()
        {
            if(gizmos == GizmoMode.Nothing || mesh == null)
                return;

            bool drawVertices = (gizmos & GizmoMode.Vertices) != 0;
            bool drawNormals = (gizmos & GizmoMode.Normals) != 0;
            bool drawTangents = (gizmos & GizmoMode.Tangents) != 0;

            if (vertices == null)
                vertices = mesh.vertices;
            if (drawNormals && normals == null)
                normals = mesh.normals;
            if (drawTangents && tangents == null)
                tangents = mesh.tangents;

            Transform t = transform;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 postion = t.TransformPoint(vertices[i]);
                if (drawVertices)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(postion, 0.02f);
                }

                if (drawNormals)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(postion, t.TransformDirection(normals[i] * 0.2f));
                }

                if (drawTangents)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(postion, t.TransformDirection(tangents[i] * 0.2f));
                }
            }
        }

        protected void OnValidate() => enabled = true;

        private int LoopIndex(int i, int length)
        {
            return (i + length) % length;
        }
        protected virtual void Update()
        {

            GenerateMesh();
            enabled = false;

            vertices = null;
            normals = null;
            tangents = null;
            Renderer.material = materials[(int) material];
            MeshCollider.sharedMesh = mesh;
        }

        protected void GenerateMesh()
        {
            UnityEngine.Mesh.MeshDataArray meshDataArray = UnityEngine.Mesh.AllocateWritableMeshData(1);
            UnityEngine.Mesh.MeshData meshData = meshDataArray[0];
            
            jobs[(int) meshType](mesh, meshData, m_pathCreator.path, default).Complete();
            
            UnityEngine.Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        }
    }
}