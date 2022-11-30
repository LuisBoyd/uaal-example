using RCR.DataStructures;
using RCR.Enums;
using RCR.Interfaces;
using RCR.Patterns;
using UnityEngine;

namespace Gameplay
{
    [System.Serializable]
    public class BuildingModel : BaseModel, IBuildingModel
    {
        [Header("In MVC Unit Details")]
        public BuildingType buildingType;


        public ObservableData<int> QueueLengthLevel { get; set; }
        public ObservableData<int> ServiceSpeedLevel { get; set; }
        public ObservableData<int> ServiceCapacityLevel { get; set; }
        public ObservableData<int> ServiceCostLevel { get; set; }
        public ObservableData<Texture2D> BuildingTexture { get; set; }
        public ObservableData<int> TimeLeftToBuild { get; set; }
    }
}