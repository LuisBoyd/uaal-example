
using UnityEngine;

namespace RCRCoreLib.Core.UI.UISystem
{
    public abstract class UIMenuRoot : UIAnimated
    {
        public abstract void Activate();
        public abstract void DeActivate();
    }
}