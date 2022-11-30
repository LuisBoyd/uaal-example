using RCR.DataStructures;
using UnityEngine;

namespace RCR.Interfaces
{
    public interface IBuildingModel
    {
        public ObservableData<int> QueueLengthLevel { get; set; }
        public ObservableData<int> ServiceSpeedLevel { get; set; }
        public ObservableData<int> ServiceCapacityLevel { get; set; }
        public ObservableData<int> ServiceCostLevel { get; set; }
        
        public ObservableData<Texture2D> BuildingTexture { get; set; }
        
        /// <summary>
        /// Time Left To Build is in seconds
        /// </summary>
        public ObservableData<int> TimeLeftToBuild { get; set; }

    }
}