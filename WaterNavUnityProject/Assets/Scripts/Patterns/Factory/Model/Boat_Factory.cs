using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.TaskSystem;
using UnityEngine;

namespace Patterns.Factory.Model
{
    public class Boat_Factory: Factory<Boat>
    {
        [SerializeField] 
        private Boat[] BoatPrefabs;
        public override Boat Create()
        {
            return Instantiate(BoatPrefabs[Random.Range(0, BoatPrefabs.Length)]);
        }

        public override Boat Clone(Boat original)
        {
            throw new System.NotImplementedException();
        }
    }
}