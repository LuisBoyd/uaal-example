using RCR.Settings.NewScripts.Entity;

namespace Events.Library.Models
{
    public class WorldEvent : BaseEvent
    {
        public class BoatSpawnRequest : WorldEvent
        {
            //Should not need more as it's just at one location
        }
        public class CustomerSpawnRequest: WorldEvent
        {
            public int Quantity;
            
            public CustomerSpawnRequest(Boat requestingBoat ,int quantity = 0)
            {
                Quantity = quantity;
                //Get Boat Docked Location to spawn things at TODO
            }
        }
        
        public class CustomerDeSpawnRequest: WorldEvent
        {
            public Customer entity;
            
            public CustomerDeSpawnRequest(Customer entity)
            {
                this.entity = entity;
            }
        }
        
        public class BoatDeSpawnRequest: WorldEvent
        {
            public Boat entity;
            
            public BoatDeSpawnRequest(Boat entity)
            {
                this.entity = entity;
            }
        }
        
    }
}