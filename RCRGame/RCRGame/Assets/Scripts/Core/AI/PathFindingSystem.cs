using System;
using System.Collections;
using System.Collections.Generic;
using RCRCoreLib.Core.Building;
using RCRCoreLib.Core.Systems;
using RCRCoreLib.Core.Tiles;
using RCRCoreLib.Core.Tiles.TilemapSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.AI
{
    public class PathFindingSystem : Singelton<PathFindingSystem>, ISystem
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

        [SerializeField] 
        private Grid Maingrid;
        
        [SerializeField]
        public List<Tilemap> maps = new List<Tilemap>();
        public List<TileBase> WaterTile = new List<TileBase>();
        public List<TileBase> LandTile = new List<TileBase>();

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
            GameManager.Instance.RegisterSystem(SystemType.PathFindingSystem, this);
            foreach (Tilemap map in maps)
            {
                foreach (var Pos in map.cellBounds.allPositionsWithin)
                {
                    //TileBase t = map.GetTile(Pos);
                    WorldTile WorldTileAtLocation = map.GetTile<WorldTile>(Pos);
                    if ((WorldTileAtLocation.pathAffector & WorldTilePathAffector.WaterPathFindable) == WorldTilePathAffector.WaterPathFindable)
                    {
                        SetWaterTilePosition(new Vector2Int(Pos.x, Pos.y));
                    }

                    if ((WorldTileAtLocation.pathAffector & WorldTilePathAffector.LandPathFindable) ==
                        WorldTilePathAffector.LandPathFindable)
                    {
                        SetLandTilePosition(new Vector2Int(Pos.x, Pos.y));
                    }
                }
            }
        }

        public void SetWaterTilePositions(IEnumerable<Vector2Int> coords)
        {
            foreach (Vector2Int coord in coords)
            {
                SetWaterTilePosition(coord);
            }
        }
        public void SetLandTilePositions(IEnumerable<Vector2Int> coords)
        {
            foreach (Vector2Int coord in coords)
            {
                SetLandTilePosition(coord);
            }
        }

        public void SetWaterTilePosition(Vector2Int coord)
        {
            if (!WatertilePositions.Contains(coord))
                WatertilePositions.Add(coord);
        }
        
        public void RemoveWaterTilesPosition(IEnumerable<Vector2Int> coords)
        {
            foreach (Vector2Int coord in coords)
            {
                RemoveWaterTilePosition(coord);
            }
        }
        
        public void RemoveLandTilesPosition(IEnumerable<Vector2Int> coords)
        {
            foreach (Vector2Int coord in coords)
            {
                RemoveLandTilePosition(coord);
            }
        }

        public void RemoveWaterTilePosition(Vector2Int coord)
        {
            if (WatertilePositions.Contains(coord))
                WatertilePositions.Remove(coord);
        }

        public void SetLandTilePosition(Vector2Int coord)
        {
            if (!PathtilePositions.Contains(coord))
                PathtilePositions.Add(coord);
        }

        public void RemoveLandTilePosition(Vector2Int coord)
        {
            if (PathtilePositions.Contains(coord))
                PathtilePositions.Remove(coord);
        }

        public void ClearTilePositions()
        {
            WatertilePositions.Clear();
            PathtilePositions.Clear();
        }

        public List<Vector3> FindPath(Vector3 startLocation, Vector3 endLocation, PathFindingMode mode)
        {
            obstacles.Clear();
            Vector3Int StartCell = new Vector3Int((int)startLocation.x, (int)startLocation.y);
            Vector3Int EndCell = new Vector3Int((int)endLocation.x, (int)endLocation.y);
            int2 StartCoord = new int2() { x = StartCell.x, y = StartCell.y };
            int2 EndCoord = new int2() { x = EndCell.x, y = EndCell.y };
            Vector2Int StartCell2D = new Vector2Int(StartCoord.x, StartCoord.y);
            Vector2Int EndCell2D = new Vector2Int(EndCoord.x, EndCoord.y);
            List<Vector3> Path = new List<Vector3>();
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
                        bool one = !WatertilePositions.Contains(StartCell2D);
                        bool two = !WatertilePositions.Contains(EndCell2D);
                        bool three = PathtilePositions.Contains(StartCell2D);
                        bool four = PathtilePositions.Contains(EndCell2D);
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
            // for (int i = 0; i < nodeArray.Length; i++)
            // {
            //     Vector3Int currentNode = new Vector3Int(nodeArray[i].coord.x,
            //         nodeArray[i].coord.y);
            //     if (!start.coord.Equals(nodeArray[i].coord) &&
            //         !end.coord.Equals(nodeArray[i].coord) &&
            //         !obstacles.ContainsKey(nodeArray[i].coord))
            //     {
            //         //Others
            //     }
            // } //EDITIDE

            if (nodes.ContainsKey(end.coord))
            {
                int2 currentCoord = end.coord;
                while (!currentCoord.Equals(start.coord))
                {
                    currentCoord = nodes[currentCoord].parent;
                    Vector3 currentTileWorldPos = Maingrid.CellToWorld(new Vector3Int(currentCoord.x, currentCoord.y)); 
                    Path.Add(currentTileWorldPos);
                }
            }

            nodes.Dispose();
            openset.Dispose();
            isObstacle.Dispose();
            offsets.Dispose();
            nodeArray.Dispose();
            Path.Reverse();
            return Path;
        }
        public List<Vector3> FindPath(Transform startlocation, Transform endlocation, PathFindingMode mode)
        {
            Vector3Int StartCell = Maingrid.WorldToCell(startlocation.position);
            Vector3Int EndCell = Maingrid.WorldToCell(endlocation.position);
            return FindPath(StartCell, EndCell, mode);
        }

        public List<Vector3> FindPath(Transform startlocation, Vector3 endlocation, PathFindingMode mode)
        {
            Vector3Int StartCell = Maingrid.WorldToCell(startlocation.position);
            Vector3Int EndCell = Maingrid.WorldToCell(endlocation);
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

        public void EnableSystem()
        {
            throw new NotImplementedException();
        }

        public void DisableSystem()
        {
            throw new NotImplementedException();
        }
    }
}