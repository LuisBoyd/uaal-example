using System;

namespace RuntimeModels
{
    [Serializable]
    public class LockedBuilding
    {
        public string name { get; set; }
        public int ID { get; set; }
        public bool Unlocked { get; set; }
        public string prefabPath { get; set; }
        public string SpriteIconPath { get; set; }
        public BuildingLevel[] BuildingLevelInfo { get; set; }
        
        [Serializable]
        public class BuildingLevel
        {
            public int CardsToNextLevel { get; set; }
            public string costToBuildType { get; set; }
            public int CostToBuild { get; set; }
            public string costToUpgradeType { get; set; }
            public int CostToUpgrade { get; set; }
            public string IconSpritePath { get; set; }
            public string PrefabPath { get; set; }
        }
    }
}