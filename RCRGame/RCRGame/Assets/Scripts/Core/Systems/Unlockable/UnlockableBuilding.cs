using System.Collections.Generic;
using RCRCoreLib.Core.Shopping.Category;

namespace RCRCoreLib.Core.Systems.Unlockable
{
    public class UnlockableBuilding: UnlockablePlaceables
    {
        //These Unlockable's have levels associated with them the player collects cards that when they reach a certain point
        //can be used to increase the level of the building's currently on the map and the new buildings they build.

        public Dictionary<int, StructureLevel> BuildingLevelInfo
            = new Dictionary<int, StructureLevel>();

        public BuildingCategory category;

        public int CurrentLevel { get; private set; }

        private string _spriteIconPath;
        public override string SpriteIconPath
        {
            get
            {
                StructureLevel currentStructureLevel = BuildingLevelInfo[CurrentLevel];
                return currentStructureLevel.IconSpritePath;
            }
            //TODO Check to when De-serialized this is fine.
        }
        public override string prefabPath
        {
            get
            {
                StructureLevel currentStructureLevel = BuildingLevelInfo[CurrentLevel];
                return currentStructureLevel.PrefabPath;
            }
            //TODO Check to when De-serialized this is fine.
        }
        public StructureLevel CurrentLevelInfo
        {
            get => BuildingLevelInfo[CurrentLevel];
        }

        //public int CardsOwned; //How many Cards of this building type are owned.

    }
}