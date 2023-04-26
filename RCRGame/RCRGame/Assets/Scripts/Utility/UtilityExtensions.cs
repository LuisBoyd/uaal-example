using Core.models.maths;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utility
{
    public static class UtilityExtensions
    {
        public static Line ConvertLineToWorldPosition(this Tilemap tilemap, Line line)
        {
            return new Line(tilemap.CellToWorld(line._startPoint.RoundUpVector()),
                tilemap.CellToWorld(line._endPoint.RoundUpVector()));
        }

        public static Vector3Int RoundUpVector(this Vector3 v)
        {
            int x = Mathf.CeilToInt(v.x);
            int y = Mathf.CeilToInt(v.y);
            int z = Mathf.CeilToInt(v.z);
            return new Vector3Int(x, y, z);
        }
    }
}