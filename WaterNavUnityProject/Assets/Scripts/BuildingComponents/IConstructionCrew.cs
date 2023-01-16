using BuildingComponents.ScriptableObjects;

namespace BuildingComponents
{
    public interface IConstructionCrew
    {
        //Get ID back from the process just to keep track...
        int StartConstruction(BuildingView View,PrestigeXBuildingObject buildingObject);
        //Generally when the app closes to save data
        void PauseAllConstruction();
        void SkipConstruction(BuildingController controller);

        bool ConstructionExists(BuildingView view);
    }
}