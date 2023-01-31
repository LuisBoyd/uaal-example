using RCR.Patterns;
using RCR.Settings.NewScripts.AI;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public class EntityController: BaseController<EntityAttributes>
    {
        
        //Need Some Access to the AILayer for movement stuff and awareness
        protected AILayer aiLayer;
        
        public override void Setup(EntityAttributes model)
        {
            base.Setup(model);
        }

        public virtual Sprite GetSprite()
        {
            return null;
        }
    }
}