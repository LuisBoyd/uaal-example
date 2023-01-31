using System;
using NewManagers;
using RCR.Patterns;
using UnityEngine;

namespace RCR.Settings.NewScripts.Entity
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Entity : BaseView<EntityAttributes, EntityController>
    {
        #region Public Methods
        public void ChangeEntityBehaviour(EntityController controller,
            EntityAttributes attributes)
        {
            if (controller != null && attributes != null)
            {
                Controller = controller;
                Model = attributes;
            }
        }
        #endregion

        #region private Methods
        private Sprite ChangeSprite(Type type)
        {
            return GameManager_2_0.Instance.SpriteDicitonary[type];
        }
        #endregion

        #region UnityFunctions
        
        #endregion
        
        
    }
}