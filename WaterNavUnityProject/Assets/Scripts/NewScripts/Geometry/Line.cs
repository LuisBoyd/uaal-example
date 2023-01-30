using System;
using UnityEngine;

namespace RCR.Settings.NewScripts.Geometry
{
    public struct Line
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;

        public Line(int x1, int y1, int x2, int y2)
        {
            StartPosition = new Vector2(x1, y1);
            EndPosition = new Vector2(x2, y2);
        }
        public Line(float x1, float y1, float x2, float y2)
        {
            StartPosition = new Vector2(x1, y1);
            EndPosition = new Vector2(x2, y2);
        }
        public Line(Vector2 start, Vector2 end)
        {
            StartPosition = start;
            EndPosition = end;
        }
        
    }
}