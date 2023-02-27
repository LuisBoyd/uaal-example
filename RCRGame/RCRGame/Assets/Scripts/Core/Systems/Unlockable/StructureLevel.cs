using System;
using RCRCoreLib.Core.Enums;

namespace RCRCoreLib.Core.Systems.Unlockable
{
    [Serializable]
    public class StructureLevel
    {
        public int CardsToNextLevel; //How many cards are needed to be able to go to the next level.

        public CurrencyType costToBuildType; //What Kind of currency is being used to Build
        public int CostToBuild; // The Cost of building

        public CurrencyType costToUpgradeType; //What kind of currency is being used to Upgrade
        public int CostToUpgrade;

        public string IconSpritePath; //Where is the location of the IconSprite
        public string PrefabPath; //Where is the location for the prefab.

    }
}