using Patterns.Factory.Model;
using RCR.Settings.NewScripts.Entity;
using UnityEngine;

namespace Patterns.ObjectPooling.Model
{
    [RequireComponent(typeof(CustomerFactory))]
    public class CustomerPool : ComponentPool<Customer>
    {
        protected override void Awake()
        {
            base.Awake();
            Factory = GetComponent<CustomerFactory>();
        }
    }
}