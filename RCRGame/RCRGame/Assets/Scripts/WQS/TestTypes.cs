using System;

namespace WQS
{
    [Flags]
    public enum TestTypes
    {
        Distance = 0,
        Dot = 1,
        GameTag = 2,
        OverlapSphere = 4,
        OverlapCube = 8,
        OverlapCylinder = 16,
        PathFinding = 32,
        Trace = 64,
    }
}