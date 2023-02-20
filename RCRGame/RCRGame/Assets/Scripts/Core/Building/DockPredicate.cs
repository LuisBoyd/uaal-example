using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Building
{
    public class DockPredicate : BuildingPredicate
    {
        [SerializeField] private TileBase WaterTile;
        [SerializeField] private TileBase LandTile;

        public override bool CanBuildHere(BoundsInt area)
        {
            TileBase[] TilesUnderneath = BuildingSystem.Instance.GetTilesBlock(area);
            bool UnderneathWater = TilesUnderneath.Any(x => x == WaterTile);
            bool UnderneathLand = TilesUnderneath.Any(y => y == LandTile);

            if (!UnderneathLand)
            {
                Debug.LogWarning("Building not underneathLand");
            }

            if (!UnderneathWater)
            {
                Debug.LogWarning("Building not UnderneathWater");
            }
            
            return UnderneathWater && UnderneathLand;
        }
    }
}