using System;
using System.Collections.Generic;
using RCR.Settings.NewScripts.Geometry;

namespace RCR.Settings.Collections.Sorting
{
    public class LineOrderByConnectivity : Comparer<Line>
    {
        public override int Compare(Line x, Line y)
        {
            if (x.StartPosition == y.StartPosition && x.EndPosition == y.EndPosition)
                return 0;
            if (x.EndPosition == y.StartPosition)
                return -1;
            if (x.StartPosition == y.EndPosition)
                return 1;
            
            return 0;
        }
    }
}