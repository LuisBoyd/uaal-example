﻿using System;
using Events.Library.Models;
using Events.Library.Models.WorldEvents;
using NewManagers;
using NewScripts.Model;
using RCR.Patterns;
using RCR.Settings.NewScripts.AI;
using RCR.Settings.NewScripts.Geometry;
using RCR.Settings.NewScripts.Tilesets;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Tile = NewScripts.Model.Tile;

namespace RCR.Settings.NewScripts.Controllers
{
    public sealed class ChunkController: BaseController<Chunk>
    {
        #region Constrcutor

        public ChunkController()
        {
            Setup(new Chunk());
#if UNITY_EDITOR
            DebugColor = Random.ColorHSV();
#endif
            //OnTilemapChanged = GameManager_2_0.Instance.EventBus.Subscribe<TilemapChanged>(On_WorldTilemapChanged);
        }

        ~ChunkController()
        {
            GameManager_2_0.Instance.EventBus.UnSubscribe<TilemapChanged>(OnTilemapChanged.TokenId);
        }
        
        #endregion
#if UNITY_EDITOR
        
        #region DebugData

        public Color DebugColor;

        #endregion
#endif

        #region Varibles

        private TilemapController tilemapController;
        private Token OnTilemapChanged;
        #endregion

        #region StartUpMethods

        // public void Read

        #endregion
        
        #region Public Methods

        public void SetChunkData(int Xorgin, int Yorigin, int width, int height)
        {
            if (!ValidateChunkPosition(new Vector2Int(Xorgin, Yorigin),
                    width, height))
                return;

            //TODO ValidationCheck to make sure it does not intersect other chunks

            Model.tiles = new Tile[width, height];
            Model.ChunkBounds = new BoundsInt(new Vector3Int(Xorgin,Yorigin, 0), new Vector3Int(Xorgin, Yorigin, 0));
            // Model.ChunkAiLayer = new AILayer(width, height);
            // Model.PathFindingSystem = new PathFindingSystem(Model.ChunkAiLayer);
            this.tilemapController = tilemapController;
        }

        public bool SetChunkVisuals(ref Tilemap toCopy)
        {
            if (!Model.HasBeenInitialized)
            {
                Debug.LogWarning($"This Chunk has not yet been Initialized");
                return false;
            }
            
            //TileBase[] tiles = ReadChunk(toCopy);
            // if (tiles == null)
            //     return false;

            //BoundsInt bounds = GetChunksBoundsInt();
            
            //tilemapController.SetTilesBlock(bounds, tiles);
            // Model.Active = true;
            // if (!Model.World.setWorldPoint)
            // {
            //     Model.World.WorldPlayerStartPoint = Model.ChunkPlayerStartingPoint;
            //     Model.World.setWorldPoint = true;
            // }
            // GameManager_2_0.Instance.EventBus.Publish(new ChunkChanged(),
            //     EventArgs.Empty);
            // GameManager_2_0.Instance.EventBus.Publish(new TilemapChanged(new TilemapChangedEventArgs(
            //        GetChunksBoundsInt(),
            //         tiles.Length)),
            //     EventArgs.Empty);
            return true;
        }
        public bool SetChunkVisuals(ref Tilemap toCopy, BoundsInt areaTocopy)
        {
            if (!Model.HasBeenInitialized)
            {
                Debug.LogWarning($"This Chunk has not yet been Initialized");
                return false;
            }
            //
            // TileBase[] tiles = ReadChunk(toCopy, areaTocopy);
            // if (tiles == null)
            //     return false;
            // BoundsInt bounds = GetChunksBoundsInt();
            // tilemapController.SetTilesBlock(bounds,tiles);
            // Model.Active = true;
            // if (!Model.World.setWorldPoint)
            // {
            //     Model.World.WorldPlayerStartPoint = Model.ChunkPlayerStartingPoint;
            //     Model.World.setWorldPoint = true;
            // }
            GameManager_2_0.Instance.EventBus.Publish(new ChunkChanged(),
                EventArgs.Empty);
            // GameManager_2_0.Instance.EventBus.Publish(new TilemapChanged(new TilemapChangedEventArgs(
            //         GetChunksBoundsInt(),
            //         tiles.Length)),
            //     EventArgs.Empty);
            return true;
        }
        public bool SetChunkVisuals(ref Tilemap toCopy, BoundsInt areaTocopy,
            BoundsInt areaToPaste)
        {
            if (!Model.HasBeenInitialized)
            {
                Debug.LogWarning($"This Chunk has not yet been Initialized");
                return false;
            }
            
            if (!ValidateAreaInChunk(areaToPaste))
                return false;
            
            //TileBase[] tiles = ReadChunk(toCopy, areaTocopy);
            //if (tiles == null)
                return false;
            //tilemapController.SetTilesBlock(areaToPaste,tiles);
            // Model.Active = true;
            // if (!Model.World.setWorldPoint)
            // {
            //     Model.World.WorldPlayerStartPoint = Model.ChunkPlayerStartingPoint;
            //     Model.World.setWorldPoint = true;
            // }
            GameManager_2_0.Instance.EventBus.Publish(new ChunkChanged(),
                EventArgs.Empty);
            // GameManager_2_0.Instance.EventBus.Publish(new TilemapChanged(new TilemapChangedEventArgs(
            //         GetChunksBoundsInt(),
            //         tiles.Length)),
            //         EventArgs.Empty);
            return true;
        }

        public void ClearChunkVisuals()
        {
            if (!Model.HasBeenInitialized)
            {
                Debug.LogWarning($"This Chunk has not yet been Initialized");
                return;
            }
            // TileBase[] Null_tiles = new TileBase[Model.Width * Model.Height];
            // tilemapController.SetTilesBlock(GetChunksBoundsInt(), Null_tiles);
            // Model.Active = false;
            GameManager_2_0.Instance.EventBus.Publish(new ChunkChanged(),
                EventArgs.Empty);
            // GameManager_2_0.Instance.EventBus.Publish(new TilemapChanged(new TilemapChangedEventArgs(
            //         GetChunksBoundsInt(),
            //         Null_tiles.Length)),
            //     EventArgs.Empty);
        }

        public Chunk GetChunk() => Model;
        public Line[] GetChunkEdges() => null;

        public Vector2Int GetChunkPlayerStartPosition() => Vector2Int.zero;//Model.ChunkPlayerStartingPoint;

        // public bool IsChunkActive() => Model.Active;
        //
        // public void PreWarmTiles(int num) => Model.TilePool.PreWarm(num);
        //
        // public Bounds GetChunkBounds()
        // {
        //     return new Bounds(new Vector3(Model.OriginX ,
        //         Model.OriginY), new Vector3(Model.Width, Model.Height, 1));
        // }
        //
        // public BoundsInt GetChunksBoundsInt()
        // {
        //     return new BoundsInt(new Vector3Int(Model.OriginX,
        //         Model.OriginY), new Vector3Int(Model.Width, Model.Height, 1));
        // }
        //
        // private BoundsInt 
        //
        #endregion

        #region privateMethods

        #region Utilities
        
        private void GetEdges ()
        {
            
            
            // Model.Edges = new Line[4]
            // {
            //     new Line(Model.ChunkBounds.min, new Vector2(Model.ChunkBounds.)), //1 - 2
            //     new Line(new Vector2(Xorgin, Yorigin + height), new Vector2(Xorgin + width, Yorigin + height)), //2 - 3
            //     new Line(new Vector2(Xorgin + width, Yorigin + height), new Vector2(Xorgin + width, Yorigin)), //3 - 4
            //     new Line(new Vector2(Xorgin + width, Yorigin), new Vector2(Xorgin, Yorigin)), //4 - 1
            // };
        }
        
        #endregion
        
        
        private bool ValidateChunkPosition(Vector2Int origin, int width, int height)
        {
            // if (origin.x > Model.World.TileWidth || origin.y > Model.World.TileHeight // This check needs to be the world width in tiles
            //                              || origin.x < 0 || origin.y < 0)
            // {
            //     Debug.LogWarning($"The inputted origin {origin} is outside the world " +
            //                      $"limits");
            //     return false;
            // }

            // if (origin.x + width > Model.World.TileWidth || origin.y + height > Model.World.TileHeight
            //                                          || height < 0 || width < 0)
            // {
            //     Debug.LogWarning($"The inputted width/height will take the bounds of this chunk " +
            //                      $"outside the world limits");
            //     return false;
            // }

            return true;
        }
        
        private bool ValidateChunkSize(int width, int height)
        {
            //return (width <= Model.Width && height <= Model.Height && height > 0 && width > 0);
            return false;
        }

        private bool ValidateChunkSize(Vector3Int size)
        {
            return ValidateChunkSize(size.x, size.y);
        }

        private bool ValidateTileBaseSize(int length)
        {
            //return length == Model.Width * Model.Height;
            return false;
        }

        private bool ValidateTileBaseSize(int length, int expectedSize)
        {
            return length == expectedSize;
        }

        private bool ValidateAreaInChunk(BoundsInt bounds)
        {
            // return (bounds.position.x < Model.Width) && (bounds.position.y < Model.Height)
            //                                          && (bounds.position.x >= Model.OriginX) &&
            //                                          bounds.position.y >= Model.OriginY
            //                                          && (bounds.position.x + bounds.size.x < Model.Width) &&
            //                                          (bounds.position.y
            //                                              + bounds.size.y < Model.Height);
        //     return (bounds.position.x < Model.OriginX + Model.Width) &&
        //            (bounds.position.y < Model.OriginY + Model.Height) &&
        //            (bounds.position.y >= Model.OriginY) && (bounds.position.x >= Model.OriginX);
        // }

        // private bool PointInChunk(Vector3 point)
        // {
        //     return GetChunkBounds().Contains(point);
        // }
        // private bool PointInChunk(Vector2 point)
        // {
        //     return GetChunkBounds().Contains(point);
        // }
        //
        // private TileBase[] ReadChunk(Tilemap tilemap)
        // {
        //     if (!ValidateChunkSize(tilemap.size))
        //         return null;
        //
        //     TileBase[] tileBases = tilemap.GetTilesBlock(tilemap.cellBounds);
        //
        //     if (!ValidateTileBaseSize(tileBases.Length) || tileBases == null)
        //         return null;
        //
        //     return tileBases;
        // }
        //
        // private void InsertLogicTiles(BoundsInt bounds,LogicTile[] logicTiles)
        // {
        //     if(!ValidateAreaInChunk(bounds))
        //         return;
        //     foreach (var vector3Int in bounds.allPositionsWithin)
        //     {
        //         //Model.tiles[vector3Int.x,vector3Int.y] = 
        //             
        //     }
        // }
        //
        // private TileBase[] ReadChunk(Tilemap tilemap, BoundsInt CopyArea)
        // {
        //     // if (!ValidateChunkSize(tilemap.size))
        //     //     return null;
        //     //Should not need validation here because If you copy a specific rect assume the
        //     //person will know what they are doing
        //     
        //     TileBase[] tileBases = tilemap.GetTilesBlock(CopyArea);
        //
        //     if (!ValidateTileBaseSize(tileBases.Length, 
        //             CopyArea.size.x * CopyArea.size.y) || tileBases == null)
        //         return null;
        //
        //     return tileBases;
        // }
        //
        // private void On_WorldTilemapChanged(TilemapChanged evnt, EventArgs args)
        // {
        //     if (!ValidateAreaInChunk(evnt.Args.Bounds))
        //         return; //Means it's not our chunk that this event is calling upon
        //     //Ai Stuff
        //     if (Model.ChunkAiLayer.ValidateAreaInAiLayer(evnt.Args.Bounds))
        //     {
        //         //Get the Underlying Tiles at the positions
        //         // PureLogicTile[] logicTiles = tilemapController.GetLogicTiles(evnt.Args.Bounds);
        //         // Model.ChunkAiLayer.ChangeAITiles(logicTiles);
        //     }
        return false;
        }
        #endregion
    }
}