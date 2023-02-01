using System.Linq;
using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.Tilesets;
using RCR.Settings.SuperNewScripts;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.WSA;

namespace RCR.Settings.NewScripts.AI
{
    public class PathFindingSystem : MultithreadedSafeSingelton<PathFindingSystem>
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 10;

        private AILayer.AITile[,] Tiles;
        private int2 gridSize;
        
        public PathFindingSystem(AILayer layer)
        {
            Tiles = layer.GetGrid();
            gridSize = new int2(Tiles.GetLength(0), Tiles.GetLength(1));
        }
        
        public PathFindingSystem(){}
        

        public void FindPath(Vector2Int startPos, Vector2Int endpos, EntityType entityType)
        {
            startPos.x = startPos.x % gridSize.x;
            startPos.y = startPos.y % gridSize.y;
            endpos.x = endpos.x % gridSize.x;
            endpos.y = endpos.y % gridSize.y;
            float startTime = Time.realtimeSinceStartup;
            NativeArray<int> GridData = new NativeArray<int>(gridSize.x * gridSize.y, Allocator.Temp);
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    //GridData[x * gridSize.x + y] = (int) Tiles[x, y].Functinailty;
                }
            }

            FindPathJob findPathJob = new FindPathJob()
            {
                StartPosition = new int2(startPos.x, startPos.y),
                EndPosition = new int2(endpos.x, endpos.y),
                EntityID = (int) entityType,
                gridSize = new int2(gridSize.x, gridSize.y),
                input = GridData,
            };

            JobHandle PathHandle = findPathJob.Schedule();
            PathHandle.Complete();
            Debug.Log($"Time: {(Time.realtimeSinceStartup - startTime) * 1000f}");

        }
        
        private struct PathNode
        {
            public int x;
            public int y;

            public int index;

            public int gCost;
            public int hCost;
            public int fCost;

            public int cameFromNodeIndex;

            public bool isWalkable;

            public void CalculateFCost()
            {
                fCost = gCost + hCost;
            }
        }
        
        private struct FindPathJob: IJob
        {
            public int2 StartPosition;
            public int2 EndPosition;
            public int EntityID;
            public int2 gridSize;

            [ReadOnly] 
            public NativeArray<int> input;

            public void Execute()
            {
                  NativeArray<PathNode> pathNodeArray =
                new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);
            for (int x = 0; x < gridSize.x ; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    PathNode pathNode = new PathNode();
                    // pathNode.x = Tiles[x, y].location.x;
                    // pathNode.y = Tiles[x, y].location.y;
                    pathNode.x = x;
                    pathNode.y = y;
                    pathNode.index = CalculateIndex(x, y, gridSize.x);

                    pathNode.gCost = int.MaxValue;
                    pathNode.hCost = CalculateDistanceCost(new int2(x,y),
                        EndPosition);
                    pathNode.CalculateFCost();

                    //Customer is set to 1 in the EntityTpe class if this changes I need to change this code
                    //boat is set to 2 in the EntityTpe class if this changes I need to change this code
                    
                    switch (EntityID)
                    {
                        case 1:
                            if (IsBitSet(input[x * gridSize.x + y],0)) //Is GroundPath bit set
                                pathNode.isWalkable = true;
                            else
                                pathNode.isWalkable = false;
                            break;
                        case 2:
                            if (IsBitSet(input[x * gridSize.x + y],1)) //Is WaterPath bit set
                                pathNode.isWalkable = true;
                            else
                                pathNode.isWalkable = false;
                            break;
                    }

                    pathNode.cameFromNodeIndex = -1;
                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            NativeArray<int2> neighborOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
            neighborOffsetArray[0] = new int2(-1, 0); //Left
            neighborOffsetArray[0] = new int2(+1, 0); //Right
            neighborOffsetArray[0] = new int2(0, +1); //up
            neighborOffsetArray[0] = new int2(0, -1); //Down
            neighborOffsetArray[0] = new int2(-1, -1); //Down Left
            neighborOffsetArray[0] = new int2(-1, +1); // Down Right
            neighborOffsetArray[0] = new int2(+1, -1); //Up Left
            neighborOffsetArray[0] = new int2(+1, +1); //Up Right

            int endNodeIndex = CalculateIndex(EndPosition.x, EndPosition.y, gridSize.x);
            PathNode startNode = pathNodeArray[CalculateIndex(StartPosition.x, StartPosition.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> ClosedList = new NativeList<int>(Allocator.Temp);
            
            openList.Add(startNode.index);

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex)
                {
                    //Reached
                    break;
                }

                //remove CurrentNode from openList
                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }
                
                ClosedList.Add(currentNodeIndex);

                for (int i = 0; i < neighborOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighborOffsetArray[i];
                    int2 neighborPosition =
                        new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsPositionInsideGrid(neighborPosition, gridSize))
                    {
                        //Neighbor Not validPostion
                        continue;
                    }

                    int neighborNodeIndex = CalculateIndex(neighborPosition.x, neighborPosition.y, gridSize.x);
                    if (ClosedList.Contains(neighborNodeIndex))
                    {
                        //Already Visited
                        continue;
                    }

                    PathNode neighborNode = pathNodeArray[neighborNodeIndex];
                    if (!neighborNode.isWalkable)
                    {
                        //Not walkable
                        continue;
                    }

                    int2 CurrentNodePosition = new int2(currentNode.x, currentNode.y);

                    int tentativeGCost =
                        currentNode.gCost + CalculateDistanceCost(CurrentNodePosition, neighborPosition);
                    if (tentativeGCost < neighborNode.gCost)
                    {
                        neighborNode.cameFromNodeIndex = currentNodeIndex;
                        neighborNode.gCost = tentativeGCost;
                        neighborNode.CalculateFCost();
                        pathNodeArray[neighborNodeIndex] = neighborNode;

                        if (!openList.Contains(neighborNode.index))
                        {
                            openList.Add(neighborNode.index);
                        }
                    }
                }
            }

            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1)
            {
                //Did not find a path
            }
            else
            {
                //Found a path!
                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
                path.Dispose();
            }
            
            pathNodeArray.Dispose();
            ClosedList.Dispose();
            openList.Dispose();
            neighborOffsetArray.Dispose();
            }
            
            private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
            {
                PathNode lowestCostPathNode = pathNodeArray[openList[0]];
                for (int i = 1; i < openList.Length; i++)
                {
                    PathNode testpathNode = pathNodeArray[openList[i]];
                    if (testpathNode.fCost < lowestCostPathNode.fCost)
                    {
                        lowestCostPathNode = testpathNode;
                    }
                }

                return lowestCostPathNode.index;
            }

            private bool IsBitSet(int b, int pos)
            {
                return ((b >> pos) & 1) != 0;
            }
            private int CalculateIndex(int x, int y, int gridwidth)
            {
                return x + y * gridwidth;
            }
            private NativeList<int2> CalculatePath(NativeArray<PathNode> pathnodeArray, PathNode endNode)
            {
                if (endNode.cameFromNodeIndex == -1)
                {
                    //Could not find a path
                    return new NativeList<int2>(Allocator.Temp);
                }
                else
                {
                    //Found a path
                    NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                    path.Add(new int2(endNode.x, endNode.y));

                    PathNode curretNode = endNode;
                    while (curretNode.cameFromNodeIndex != -1)
                    {
                        PathNode cameFromNode = pathnodeArray[curretNode.cameFromNodeIndex];
                        path.Add(new int2(cameFromNode.x, cameFromNode.y));
                        curretNode = cameFromNode;
                    }

                    return path;
                }
            }
            private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
            {
                int xDistance = math.abs(aPosition.x - bPosition.x);
                int yDistance = math.abs(aPosition.y - bPosition.y);
                int remaining = math.abs(xDistance - yDistance);
                return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
            }
            private bool IsPositionInsideGrid(int2 gridPosition, int2 GridSize)
            {
                return gridPosition.x >= 0 &&
                       gridPosition.y >= 0 &&
                       gridPosition.x < gridSize.x &&
                       gridPosition.y < gridSize.y;
            }

        }
    }
}