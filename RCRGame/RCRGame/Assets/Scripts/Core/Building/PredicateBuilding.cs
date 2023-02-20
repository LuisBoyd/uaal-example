using System.Collections.Generic;
using UnityEngine;

namespace RCRCoreLib.Core.Building
{
    public class PredicateBuilding : Building
    {
        [SerializeField] 
        private List<BuildingPredicate> BuildingPredicates = new List<BuildingPredicate>();

        public override bool CanBePlaced()
        {
            Vector3Int positionInt = BuildingSystem.Instance.gridLayout.LocalToCell(OriginTransform.position);
            BoundsInt areaTemp = new BoundsInt(positionInt, new Vector3Int(area.size.x, area.size.y, 1));
            return BuildingSystem.Instance.CanTakeAreaWithPredicates(areaTemp, BuildingPredicates);
        }
    }
}