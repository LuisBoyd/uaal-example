using RCRCoreLib.Core.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Optimisation.Patterns.Command
{
    public interface ITileCommandHandler : ICommandHandler<TileCommand>
    {
        void PlaceTile(Vector3Int cellPos ,TileBase tile);
        void PlaceTiles(BoundsInt bounds ,TileBase[] tilesBases);
        
        void RemoveTile(Vector3Int cellPos);
        WorldTile CheckTile(Vector3Int cellPos);
    }
}