using Patterns.Factory;
using Patterns.Factory.Model;
using RCR.Settings.NewScripts.Entity;
using UnityEngine;

namespace Patterns.ObjectPooling.Model
{
    [RequireComponent(typeof(Factory_Entity))]
    public class EntityPool: ComponentPool<Entity>
    {
        public override IFactory<Entity> Factory { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Factory = GetComponent<Factory_Entity>();
        }
    }
}