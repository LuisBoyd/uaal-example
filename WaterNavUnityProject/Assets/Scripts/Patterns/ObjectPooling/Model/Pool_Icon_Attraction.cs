using Patterns.Factory;
using Patterns.Factory.Model;
using UI.uGUI;
using UnityEngine;

namespace Patterns.ObjectPooling.Model
{
    [RequireComponent(typeof(Factory_Icon_Attraction))]
    public class Pool_Icon_Attraction : ComponentPool<Icon_Attraction>
    {

        public override IFactory<Icon_Attraction> Factory { get; set; }
        
        protected override void Awake()
        {
            base.Awake();
            Factory = GetComponent<Factory_Icon_Attraction>();
        }
    }
}