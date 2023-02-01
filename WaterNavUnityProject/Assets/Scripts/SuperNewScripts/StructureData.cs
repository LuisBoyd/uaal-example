using UnityEngine;

namespace RCR.Settings.SuperNewScripts
{
    public class StructureData
    {
        /// <summary>
        /// The Space that the Structure Takes up on the grid
        /// </summary>
        public BoundsInt GridBounds { get; private set; }
        
        /// <summary>
        /// The StructureID is a lookup value against a table of pre-existing
        ///  Structure's 
        /// </summary>
        public int StructureID { get; private set; }
        
        /// <summary>
        /// A bitmasked integer that can represent multiple effects applied to the building
        /// </summary>
        public int BitMaskedEffectsID { get; private set; }

        #region UpgradeData
        /// <summary>
        /// The Overall Level of this Structure Query this for bonuses to the base Structure 
        /// </summary>
        public int StructureOverallLevel { get; private set; }
        /*
         * All the Upgrades below are stand in place one's it just
         * means that every structure can have up to 8 upgrades that
         * have different effects for it, again we can query what
         * each upgrade index for a certain building ID means if we have a
         * pre-Existing external list
         */
        public int UpgradeOneLevel { get; private set; }
        public int UpgradetwoLevel { get; private set; }
        public int UpgradeThreeLevel { get; private set; }
        public int UpgradeFourLevel { get; private set; }
        public int UpgradeFiveLevel { get; private set; }
        public int UpgradeSixLevel { get; private set; }
        public int UpgradeSevenLevel { get; private set; }
        public int UpgradeEightLevel { get; private set; }
        #endregion

        #region Constructors
        public StructureData(){} //Default for serialization
        #endregion
    }
}