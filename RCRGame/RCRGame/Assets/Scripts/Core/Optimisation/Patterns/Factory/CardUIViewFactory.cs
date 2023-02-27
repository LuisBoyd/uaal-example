using RCRCoreLib.Core.UI;
using Unity.Mathematics;
using UnityEngine;

namespace RCRCoreLib.Core.Optimisation.Patterns.Factory
{
    public class CardUIViewFactory : Factory<CardView>
    {
        [SerializeField]
        private CardView prefab;
        public override CardView Create()
        {
            return Instantiate(prefab, Vector3.zero, quaternion.identity);
        }
    }
}