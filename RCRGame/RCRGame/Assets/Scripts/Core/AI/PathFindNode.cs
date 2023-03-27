using Unity.Mathematics;

namespace RCRCoreLib.Core.AI
{
    public struct PathFindNode
    {
        public int2 coordinate; //X and Y Position of Vertex.
        public int2 parentCoordinate; //Used to keep track of which node the path came from.
        public int gScore; //represents the cost from the start node to this vertex.
        public int hScore; //represents the heuristic estimated cost from the end node to this vertex.
        public bool passable; //is this vertex passable in the A* algorithm.
    }
}