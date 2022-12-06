using Listners;

namespace Components
{
    public class LocationComponent : BaseComponent, IOutOfRangeListner
    {
        public void OnOutOfRange(string locationData)
        {
            //TODO Initiate Exit Game Procedure
            throw new System.NotImplementedException();
        }
    }
}