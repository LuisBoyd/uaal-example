using NewManagers;
using RCR.Patterns;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    public class CustomerController: EntityController
    {
        public override Sprite GetSprite()
        {
            return GameManager_2_0.Instance.SpriteDicitonary[typeof(CustomerAttributes)];
        }
    }
}