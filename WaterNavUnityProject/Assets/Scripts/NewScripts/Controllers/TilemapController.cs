using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events.Library.Models;
using NewScripts.Model;
using RCR.Patterns;
using RCR.Settings.NewScripts.Tilesets;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCR.Settings.NewScripts.Controllers
{
    public sealed class TilemapController : BaseController<TilemapData>
    {
        #region Event
        #endregion

        #region values
        private Dictionary<Vector3Int, PureLogicTile> logicTiles;
        #endregion

        public TilemapController()
        {
            Setup(new TilemapData());
            logicTiles = new Dictionary<Vector3Int, PureLogicTile>();
        }

        #region Public Methods
        public void SetTilemapData(Tilemap unityTilemap)
        {
            Model.tilemap = unityTilemap;
            Model.HasBeenInitialized = true;
        }

        public void RemoveTile(Vector2Int position)
        {
            if(!Model.HasBeenInitialized)
                return;
            RemoveTilePlaced(new Vector3Int(position.x,position.y));
            Model.tilemap.SetTile(new Vector3Int(
                position.x,position.y), null);
        }
        public void RemoveTile(Vector3Int position)
        {
            if(!Model.HasBeenInitialized)
                return;
            RemoveTilePlaced(position);
            Model.tilemap.SetTile(position, null);
        }
        public void SetTile(Vector2Int position, TileBase tile)
        {
            if(tile == null|| !Model.HasBeenInitialized)
                return;
            RemoveTilePlaced(new Vector3Int(position.x,position.y));
            Model.tilemap.SetTile(new Vector3Int(position.x,
                position.y), tile);
            NewTilePlaced(new Vector3Int(position.x,position.y));
            //Callback method here for checking tile stuff
        }
        public void SetTile(Vector3Int position, TileBase tile)
        {
            if(tile == null|| !Model.HasBeenInitialized)
                return;
            RemoveTilePlaced(position);
            Model.tilemap.SetTile(position, tile);
            NewTilePlaced(position);
            //Callback method here for checking tile stuff
        }
        public void SetTilesBlock(BoundsInt bounds, TileBase[] tiles)
        {
            if(tiles == null || !Model.HasBeenInitialized)
                return;
            RemoveTilesPlaced(bounds);
            Model.tilemap.SetTilesBlock(bounds, tiles);
            NewTilesPlaced(bounds);
            //Callback method here for checking tile stuff
        }

        public void CompressBounds()
        {
            if(!Model.HasBeenInitialized)
                return;
            Model.tilemap.CompressBounds();
        }

        public void ResizeBounds()
        {
            if(!Model.HasBeenInitialized)
                return;
            Model.tilemap.ResizeBounds();
        }

        public PureLogicTile[] GetLogicTiles(BoundsInt bounds)
        {
            if(!Model.HasBeenInitialized)
                return null;
            return logicTiles.Values.Where(lt => lt.location.x <= bounds.max.x
                                                 && lt.location.y <= bounds.max.y &&
                                                 lt.location.x >= bounds.min.x &&
                                                 lt.location.y >= bounds.min.y).ToArray();
        }
        
        #endregion

        #region private methods
        private void NewTilesPlaced(BoundsInt bounds)
        {
            foreach (var vector3Int in bounds.allPositionsWithin)
            {
                TileBase logicTile = Model.tilemap.GetTile(vector3Int);
                if(logicTile is not LogicTile)
                    continue;
                PureLogicTile pureLogicTile = new PureLogicTile(logicTile as LogicTile,
                    new Vector2Int(vector3Int.x, vector3Int.y));
                RegisterTileLogic(vector3Int, pureLogicTile);
            }
        }
        
        
        private void NewTilePlaced(Vector3Int point)
        {
            LogicTile logicTile = Model.tilemap.GetTile<LogicTile>(point);
            if(logicTile == null)
                return;
            PureLogicTile pureLogicTile = new PureLogicTile(logicTile, new Vector2Int(point.x,point.y));
            RegisterTileLogic(point, pureLogicTile);
        }

        private void RemoveTilesPlaced(BoundsInt bounds)
        {
            foreach (var vector3Int in bounds.allPositionsWithin)
            {
                RemoveTilePlaced(vector3Int);
            }
        }
        private void RemoveTilePlaced(Vector3Int point)
        {
            if (logicTiles.ContainsKey(point))
            {
                UnRegisterTileLogic(point);
            }
        }

        private void RegisterTileLogic(Vector3Int point ,PureLogicTile logicTile)
        {
            try
            {
                logicTiles.Add(point, logicTile);
                TileInfo info = new TileInfo(new Vector2Int(point.x, point.y), ref Model.tilemap);
                //logicTile.Controller.Start(info);
            }
            catch (ArgumentException argumentException)
            {
                Debug.LogError($"Value Exists in Table already");
            }
            catch(NotSupportedException notSupportedException)
            {
                Debug.LogError($"{point} on Tilemap is Not supported");
            }
        }

        private void UnRegisterTileLogic(Vector3Int point)
        {
            try
            {
                if (logicTiles[point] is PureLogicTile)
                {
                    //(logicTiles[point] as PureLogicTile).Controller.End();
                }
            }
            catch (ArgumentNullException e)
            {
               Debug.LogError($"The Entered key was Null");
            }
        }

        private void ProcessRegisterTileLogic(Vector3Int point)
        {
            try
            {
                if (logicTiles[point] is PureLogicTile)
                {
                    //(logicTiles[point] as PureLogicTile).Controller.Process();
                }
            }
            catch (ArgumentNullException e)
            {
                Debug.LogError($"The Entered key was Null");
            }
        }
        #endregion
    }
}