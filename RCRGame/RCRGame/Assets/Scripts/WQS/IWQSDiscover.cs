using UnityEngine;

namespace WQS
{
    /// <summary>
    /// Interface make's class discoverable to the WQS system
    /// </summary>
    public interface IWQSDiscover
    {
        bool CurrentlySelected { get; set; }
        Collider2D Collider2D { get; }
        GameObject GameObject { get; }
        float DistanceTo(Transform agent);
        float Dot(Transform agent);
        bool PathFinding(Transform agent, PathFindingMode mode, out Vector3[] resultingPath);
        bool Trace(Transform agent, out RaycastHit2D hit2D);
        bool Overlap(Transform agent, ShapeInfo shape, out Collider2D[] collider2Ds);
        bool Tag(string tag ,out GameObject obj);
    }
}