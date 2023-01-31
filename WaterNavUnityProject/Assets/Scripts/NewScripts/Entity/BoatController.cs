using NewManagers;
using RCR.Patterns;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public class BoatController: EntityController
    {
        public override void Setup(EntityAttributes model)
        {
            base.Setup(model);
        }

        public override Sprite GetSprite()
        {
            return GameManager_2_0.Instance.SpriteDicitonary[typeof(BoatAttributes)];
        }
    }
}