using Events.Library.Models;
using NewScripts.Model;
using RCR.Patterns;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCR.Settings.NewScripts.Controllers
{
    public sealed class TilemapController : BaseController<TilemapData>
    {
        #region Event
        #endregion

        public TilemapController()
        {
            Setup(new TilemapData());
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
            Model.tilemap.SetTile(new Vector3Int(
                position.x,position.y), null);
        }
        public void RemoveTile(Vector3Int position)
        {
            if(!Model.HasBeenInitialized)
                return;
            Model.tilemap.SetTile(position, null);
        }
        public void SetTile(Vector2Int position, TileBase tile)
        {
            if(tile == null|| !Model.HasBeenInitialized)
                return;
            Model.tilemap.SetTile(new Vector3Int(position.x,
                position.y), tile);
            //Callback method here for checking tile stuff
        }
        public void SetTile(Vector3Int position, TileBase tile)
        {
            if(tile == null|| !Model.HasBeenInitialized)
                return;
            Model.tilemap.SetTile(position, tile);
            //Callback method here for checking tile stuff
        }
        public void SetTilesBlock(BoundsInt bounds, TileBase[] tiles)
        {
            if(tiles == null || !Model.HasBeenInitialized)
                return;
            Model.tilemap.SetTilesBlock(bounds, tiles);
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
        #endregion

        #region private methods
        
        #endregion
    }
}