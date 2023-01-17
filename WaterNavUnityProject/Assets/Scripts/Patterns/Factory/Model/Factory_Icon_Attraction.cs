using UI.uGUI;
using UnityEngine;

namespace Patterns.Factory.Model
{
    public class Factory_Icon_Attraction : Factory<Icon_Attraction>
    {
        [SerializeField] 
        private Icon_Attraction _original;
        public override Icon_Attraction Create()
        {
            return Instantiate(_original);
        }

        public override Icon_Attraction Clone(Icon_Attraction original)
        {
            throw new System.NotImplementedException();
        }
    }
}