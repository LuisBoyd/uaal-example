using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace.Events
{
    [CreateAssetMenu(menuName = "RCR/Events/Tile Placement Event Channel")]
    public class TilePlacementEventChannelSO : EventRelayTwo<TileBase, Vector2Int>
    {
        
    }
}