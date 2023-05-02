using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Optimisation.Patterns.Factory
{
    public class EntityFactory : Factory<Entity.Entity>
    {
        [Title("Entity", TitleAlignment = TitleAlignments.Centered)] 
        [Required] [SerializeField]private Entity.Entity Preset;

        public override Entity.Entity Create()
        {
            return Instantiate(Preset, Vector3.zero, Quaternion.identity);
        }
    }
}