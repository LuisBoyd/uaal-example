using RCR.Enums;

namespace RCR.Interfaces
{
    public interface IBuildingController
    {
        public int GetQueueLength();
        public int GetServiceSpeed();
        public int GetCapacity();
        public int GetServiceCost();

        public int GetPayOut();
        public void Iterate();

        public void Upgrade(BuildingUpgrade upgrade);
        
        
    }
}