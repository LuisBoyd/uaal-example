using DataStructures;
using RCR.Enums;

namespace RCR.Gameplay
{
    public class Building: StaticObject
    {
        private BuildingData m_buildingData;
        
        protected override void Awake()
        {
            base.Awake();

            m_buildingData = new BuildingData(BuildingType.Non_Constrcuted);
        }
    }
}