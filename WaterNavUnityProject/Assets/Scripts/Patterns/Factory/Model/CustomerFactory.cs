using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.TaskSystem;
using UnityEngine;

namespace Patterns.Factory.Model
{
    public class CustomerFactory: Factory<Customer>
    {
        [SerializeField]
        private Customer[] customerPrefabs;
        public override Customer Create()
        {
            return Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Length)]);
        }

        public override Customer Clone(Customer original)
        {
            throw new System.NotImplementedException();
        }
    }
}