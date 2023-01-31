using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.Tilesets;

namespace Events.Library.Models.WorldEvents
{
    public class OnSpawnerPlaced : OnLogicChangedEvent
    {
        public EntityType entityType;
        
        public OnSpawnerPlaced(OnLogicChangedArgs args) : base(args)
        {
            switch (args.LogicValue)
            {
                default:
                    entityType = 0;
                    break;
                case LogicDecorations.BoatSpawner:
                    entityType = EntityType.Boat;
                    break;
                case LogicDecorations.CustomerSpawner:
                    entityType = EntityType.Customer;
                    break;
            }
        }
    }
}