using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCR.Settings.SuperNewScripts.World.Interfaces
{
    public interface IObservableTileMap
    {
        Tilemap Tilemap { get; }
        
        void SetTile(Vector3Int position, TileBase tile);
        void SetTiles(Vector3Int[] positions, TileBase[] tiles);
        void SetTilesBlock(BoundsInt position, TileBase[] tiles);
    }
}