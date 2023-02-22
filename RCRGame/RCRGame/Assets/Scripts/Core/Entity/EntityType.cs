using RCRCoreLib.Core.AI;
using UnityEngine;

namespace RCRCoreLib.Core.Entity
{
    [CreateAssetMenu(fileName = "New Item", menuName = "GameObjects/Entity Type")]
    public class EntityType : ScriptableObject
    {
        public string EntityName;
        public float MovementSpeed;
        public PathFindingSystem.PathFindingMode PathFindingMode;
    }
}