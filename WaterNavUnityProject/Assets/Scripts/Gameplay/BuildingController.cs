using RCR.Enums;
using RCR.Interfaces;
using RCR.Patterns;

namespace Gameplay
{
    public class BuildingController : BaseController<BuildingModel>, IBuildingController
    {
        //Controls The Logic and calculations/Desicions and either submits the
        //Infromation to the Model or returns Data to the View


        public int GetQueueLength()
        {
            throw new System.NotImplementedException();
        }

        public int GetServiceSpeed()
        {
            throw new System.NotImplementedException();
        }

        public int GetCapacity()
        {
            throw new System.NotImplementedException();
        }

        public int GetServiceCost()
        {
            throw new System.NotImplementedException();
        }

        public int GetPayOut()
        {
            throw new System.NotImplementedException();
        }

        public async void Iterate()
        {
            throw new System.NotImplementedException();
        }

        public void Upgrade(BuildingUpgrade upgrade)
        {
            throw new System.NotImplementedException();
        }

        public bool CanWeBuild()
        {
            //TODO check if basically the timer has not been accidently set and starts building by accident
            return false;
        }
    }
}