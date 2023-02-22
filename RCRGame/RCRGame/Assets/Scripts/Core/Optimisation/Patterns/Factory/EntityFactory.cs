using Unity.Mathematics;
using UnityEngine;

namespace RCRCoreLib.Core.Optimisation.Patterns.Factory
{
    public class EntityFactory: Factory<Entity.Entity>
    {
        [SerializeField]
        private Entity.Entity prefab;
        public override Entity.Entity Create()
        {
            return Instantiate(prefab, Vector3.zero, quaternion.identity);
        }
    }
}