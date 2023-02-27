using RCRCoreLib.Core.UI;
using UnityEngine;

namespace RCRCoreLib.Core.Optimisation.Patterns.ObjectPooling
{
    public class CardUIViewPool : ComponentPool<CardView>
    {
        [SerializeField] 
        private RectTransform Content;

        public override CardView Request()
        {
            CardView view = base.Request();
            view.transform.SetParent(Content, false);
            return view;
        }
    }
}