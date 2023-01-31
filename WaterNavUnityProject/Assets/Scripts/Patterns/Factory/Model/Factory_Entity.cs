using RCR.Settings.NewScripts.Entity;
using UnityEngine;

namespace Patterns.Factory.Model
{
    public class Factory_Entity: Factory<Entity>
    {
        public override Entity Create()
        {
            Entity e = new GameObject("Entity_").AddComponent<Entity>();
            return e;
        }

        public override Entity Clone(Entity original)
        {
            throw new System.NotImplementedException();
        }
    }
}