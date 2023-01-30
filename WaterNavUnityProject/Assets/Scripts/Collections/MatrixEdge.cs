using UnityEngine;

namespace RCR.Settings.Collections
{
    public class MatrixEdge<T>
    {
        public Vector2Int EdgeStartID;
        public Vector2Int EdgeEndID;
        public T EdgeStart;
        public T EdgeEnd;

        public MatrixEdge(int x1, int y1,int x2,int y2, T edgeStart, T edgeEnd)
        {
            this.EdgeStartID = new Vector2Int(x1, y1);
            this.EdgeEndID = new Vector2Int(x2, y2);
            this.EdgeStart = edgeStart;
            this.EdgeEnd = edgeEnd;
        }
    }
}