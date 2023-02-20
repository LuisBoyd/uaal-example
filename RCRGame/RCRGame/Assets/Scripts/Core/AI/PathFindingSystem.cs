using System;
using System.Collections;
using System.Collections.Generic;
using RCRCoreLib.Core.Building;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.AI
{
    public class PathFindingSystem : Singelton<PathFindingSystem>
    {
        public struct Node
        {
            public int2 coord;
            public int2 parent;
            public int gScore;
            public int hScore;
        }
        
        public enum PathFindingMode
        {
            Land = 0,
            Water = 1
        }

        private Hashtable obstacles;
        private Node start, end;
        private int safeGuard = 1000;

        public Tilemap map;
        public TileBase WaterTile;
        public TileBase LandTile;

        private HashSet<Vector2Int> WatertilePositions;
        private HashSet<Vector2Int> PathtilePositions;

        protected override void Awake()
        {
            base.Awake();
            obstacles = new Hashtable();
            WatertilePositions = new HashSet<Vector2Int>();
            PathtilePositions = new HashSet<Vector2Int>();
            start = new Node() { coord = int2.zero, parent = int2.zero, gScore = int.MaxValue, hScore = int.MaxValue };
            end = new Node() { coord = int2.zero, parent = int2.zero, gScore = int.MaxValue, hScore = int.MaxValue };
        }

        private void Start()
        {
            map = BuildingSystem.Instance.MainTilemap;
            foreach (var Pos in map.cellBounds.allPositionsWithin)
            {
                TileBase TileAtLocation = map.GetTile(Pos);
                if (TileAtLocation == WaterTile)
                    SetWaterTilePosition(new Vector2Int(Pos.x, Pos.y));
                else if (TileAtLocation == LandTile)
                    SetLandTilePosition(new Vector2Int(Pos.x, Pos.y));
            }

        }

        private void SetWaterTilePosition(Vector2Int coord)
        {
            if (!WatertilePositions.Contains(coord))
                WatertilePositions.Add(coord);
        }

        private void RemoveWaterTilePosition(Vector2Int coord)
        {
            if (WatertilePositions.Contains(coord))
                WatertilePositions.Remove(coord);
        }

        private void SetLandTilePosition(Vector2Int coord)
        {
            if (!PathtilePositions.Contains(coord))
                PathtilePositions.Add(coord);
        }

        private void RemoveLandTilePosition(Vector2Int coord)
        {
            if (PathtilePositions.Contains(coord))
                PathtilePositions.Remove(coord);
        }

        private void ClearTilePositions()
        {
            WatertilePositions.Clear();
            PathtilePositions.Clear();
        }

        public List<Vector3Int> FindPath(Vector3 startLocation, Vector3 endLocation, PathFindingMode mode)
        {
            obstacles.Clear();
            Vector3Int StartCell = new Vector3Int((int)startLocation.x, (int)startLocation.y);
            Vector3Int EndCell = new Vector3Int((int)endLocation.x, (int)endLocation.y);
            int2 StartCoord = new int2() { x = StartCell.x, y = StartCell.y };
            int2 EndCoord = new int2() { x = EndCell.x, y = EndCell.y };
            Vector2Int StartCell2D = new Vector2Int(StartCoord.x, StartCoord.y);
            Vector2Int EndCell2D = new Vector2Int(EndCoord.x, EndCoord.y);
            List<Vector3Int> Path = new List<Vector3Int>();
            start.coord = StartCoord;
            end.coord = EndCoord;
            if (obstacles.ContainsKey(StartCoord) || obstacles.ContainsKey(EndCoord))
                return null;
            switch (mode)
            {
                case PathFindingMode.Land:
                    if (WatertilePositions.Contains(StartCell2D) || WatertilePositions.Contains(EndCell2D)
                                                                 || !PathtilePositions.Contains(StartCell2D) ||
                                                                 !PathtilePositions.Contains(EndCell2D))
                    {
                        bool one = WatertilePositions.Contains(StartCell2D);
                        bool two = WatertilePositions.Contains(EndCell2D);
                        bool three = !PathtilePositions.Contains(StartCell2D);
                        bool four = !PathtilePositions.Contains(EndCell2D);
                        return null;
                    }
                    else
                    {
                        foreach (var watertilePosition in WatertilePositions)
                        {
                            obstacles.Add(watertilePosition, true);
                        }
                    }
                    break;
                case PathFindingMode.Water:
                    if (!WatertilePositions.Contains(StartCell2D) || !WatertilePositions.Contains(EndCell2D)
                                                                 || PathtilePositions.Contains(StartCell2D) ||
                                                                 PathtilePositions.Contains(EndCell2D))
                    {
                        return null;
                    }
                    else
                    {
                        foreach (var pathtilePosition in PathtilePositions)
                        {
                            obstacles.Add(pathtilePosition, true);
                        }
                    }
                    break;
                default:
                    return null;
                break;
            }
            
            //Start of Algorithm
            NativeHashMap<int2, bool> isObstacle = new NativeHashMap<int2, bool>(obstacles.Count,
                Allocator.TempJob);
            NativeHashMap<int2, Node> nodes = new NativeHashMap<int2, Node>(safeGuard, Allocator.TempJob);
            NativeHashMap<int2, Node> openset = new NativeHashMap<int2, Node>(safeGuard, Allocator.TempJob);
            NativeArray<int2> offsets = new   NativeArray<int2>(8, Allocator.TempJob);

            foreach (Vector2Int o in obstacles.Keys)
            {
                isObstacle.Add(new int2(o.x,o.y),true);
            }

            Astar astar = new Astar()
            {
                isObstacle = isObstacle,
                offsets = offsets,
                nodes = nodes,
                openSet = openset,
                start = start,
                end = end,
                safeGuard = safeGuard
            };

            JobHandle handle = astar.Schedule();
            handle.Complete();
            NativeArray<Node> nodeArray = nodes.GetValueArray(Allocator.TempJob);
            for (int i = 0; i < nodeArray.Length; i++)
            {
                Vector3Int currentNode = new Vector3Int(nodeArray[i].coord.x,
                    nodeArray[i].coord.y);
                if (!start.coord.Equals(nodeArray[i].coord) &&
                    !end.coord.Equals(nodeArray[i].coord) &&
                    !obstacles.ContainsKey(nodeArray[i].coord))
                {
                    //Others
                }
            }

            if (nodes.ContainsKey(end.coord))
            {
                int2 currentCoord = end.coord;
                while (!currentCoord.Equals(start.coord))
                {
                    currentCoord = nodes[currentCoord].parent;
                    Vector3Int currentTile = new Vector3Int(currentCoord.x,
                        currentCoord.y, 0);
                    Path.Add(currentTile);
                }
            }

            nodes.Dispose();
            openset.Dispose();
            isObstacle.Dispose();
            offsets.Dispose();
            nodeArray.Dispose();
            return Path;
        }
        public List<Vector3Int> FindPath(Transform startlocation, Transform endlocation, PathFindingMode mode)
        {
            Vector3Int StartCell = map.WorldToCell(startlocation.position);
            Vector3Int EndCell = map.WorldToCell(endlocation.position);
            return FindPath(StartCell, EndCell, mode);
        }
        //[BurstCompile(CompileSynchronously = true)]
        public struct Astar: IJob
        {
            public NativeHashMap<int2, bool> isObstacle;
            public NativeHashMap<int2, Node> nodes;
            public NativeHashMap<int2, Node> openSet;
            public NativeArray<int2> offsets;

            public Node start;
            public Node end;

            public int safeGuard;
            
            public void Execute()
            {
                Node current = start;
                current.gScore = 0;
                current.hScore = SquaredDistance(current.coord, end.coord);
                openSet.TryAdd(current.coord, current);

                offsets[0] = new int2(0, 1);
                offsets[1] = new int2(1, 1);
                offsets[2] = new int2(1, 0);
                offsets[3] = new int2(1, -1);
                offsets[4] = new int2(0, -1);
                offsets[5] = new int2(-1, -1);
                offsets[6] = new int2(-1, 0);
                offsets[7] = new int2(-1, 1);

                int counter = 0;

                do
                {
                    current = openSet[ClosestNode()];
                    nodes.TryAdd(current.coord, current);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        if (!nodes.ContainsKey(current.coord + offsets[i]) &&
                            !isObstacle.ContainsKey(current.coord + offsets[i]))
                        {
                            Node neighbor = new Node()
                            {
                                coord = current.coord + offsets[i],
                                parent = current.coord,
                                gScore = current.gScore + SquaredDistance(current.coord,
                                    current.coord + offsets[i]),
                                hScore = SquaredDistance(current.coord + offsets[i], end.coord)
                            };
                            if (openSet.ContainsKey(neighbor.coord) && neighbor.gScore <
                                openSet[neighbor.coord].gScore)
                            {
                                openSet[neighbor.coord] = neighbor;
                            }
                            else if (!openSet.ContainsKey(neighbor.coord))
                            {
                                openSet.TryAdd(neighbor.coord, neighbor);
                            }
                        }
                    }

                    openSet.Remove(current.coord);
                    counter++;
                    if(counter > safeGuard)
                        break;

                } while (openSet.Count() != 0 && !current.coord.Equals(end.coord));

            }
            public int SquaredDistance(int2 coordA, int2 coordB)
            {
                int a = coordB.x - coordA.x;
                int b = coordB.y - coordA.y;
                return a * a + b * b;
            }

            public int2 ClosestNode()
            {
                Node result = new Node();
                int fscore = int.MaxValue;

                NativeArray<Node> nodeArray = openSet.GetValueArray(Allocator.Temp);

                for (int i = 0; i < nodeArray.Length; i++)
                {
                    if (nodeArray[i].gScore + nodeArray[i].hScore < fscore)
                    {
                        result = nodeArray[i];
                        fscore = nodeArray[i].gScore + nodeArray[i].hScore;
                    }
                }

                nodeArray.Dispose();
                return result.coord;
            }
        }
        
    }
}