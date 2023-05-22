using System.Collections.Generic;
using CustomUIFramework.Organisms;
using UnityEngine;

namespace CustomUIFramework.Animation.Propagators
{
    public abstract class Propergator
    {
        [SerializeField] 
        protected List<SlicePanel> members;

        public abstract void In();
        public abstract void Out();
    }
}