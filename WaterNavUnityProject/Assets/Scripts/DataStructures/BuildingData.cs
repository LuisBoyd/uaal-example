using RCR.Enums;

namespace DataStructures
{
    public struct BuildingData
    {

        public BuildingData(BuildingType type)
        {
            BuildingType = type;
        }
        
        public BuildingType BuildingType;
    }
}