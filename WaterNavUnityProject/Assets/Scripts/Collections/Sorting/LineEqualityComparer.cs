using System.Collections.Generic;
using RCR.Settings.NewScripts.Geometry;

namespace RCR.Settings.Collections.Sorting
{
    public class LineEqualityComparer : EqualityComparer<Line>
    {
        public override bool Equals(Line x, Line y)
        {
            if (x.StartPosition == y.EndPosition &&
                y.StartPosition == x.EndPosition)
                return true;
            return false;
        }

        public override int GetHashCode(Line obj)
        {
            int hCode = (int)(obj.StartPosition.x * obj.StartPosition.y *
                        obj.EndPosition.x * obj.EndPosition.y);
            return hCode;
        }
    }
}