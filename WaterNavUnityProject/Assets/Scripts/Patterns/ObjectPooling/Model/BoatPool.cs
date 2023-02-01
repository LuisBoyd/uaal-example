using Patterns.Factory.Model;
using RCR.Settings.NewScripts.Entity;
using UnityEngine;

namespace Patterns.ObjectPooling.Model
{
    [RequireComponent(typeof(Boat_Factory))]
    public class BoatPool: ComponentPool<Boat>
    {
        protected override void Awake()
        {
            base.Awake();
            Factory = GetComponent<Boat_Factory>();
        }
    }
}