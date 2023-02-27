using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Enums;
using RCRCoreLib.Core.Shopping.Category;

namespace RCRCoreLib.Core.Systems.Unlockable
{
    [Serializable]
    public class UnlockableStructure : UnlockablePlaceables
    {
        //Unlock-able Structure is akin to something like a fence or a signPost. mainly as they can't be leveled up unlike buildings
        //and can only really be unlocked and then that's it.
        
        public CurrencyType costToBuildType; //What Kind of currency is being used to Build
        public int CostToBuild; // The Cost of building

        public DecorationCategory Category;
    }
}