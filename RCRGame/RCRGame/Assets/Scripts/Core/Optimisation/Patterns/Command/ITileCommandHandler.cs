using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Optimisation.Patterns.Command
{
    public interface ITileCommandHandler : ICommandHandler<TileCommand>
    {
        void PlaceTile(Vector3Int cellPos ,TileBase tile);
        
        void RemoveTile(Vector3Int cellPos);
        TileBase CheckTile(Vector3Int cellPos);
    }
}