using RCR.Settings.NewScripts.Entity;
using RCR.Settings.NewScripts.Tilesets;

namespace Events.Library.Models.WorldEvents
{
    public class OnSpawnerPlaced : OnLogicChangedEvent
    {
        public EntityType entityType;
        
        public OnSpawnerPlaced(OnLogicChangedArgs args) : base(args)
        {
          
        }
    }
}