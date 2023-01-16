using System;
using BuildingComponents.ScriptableObjects;

namespace Events.Library.Models.BuildingEvents
{
    public class ConcreteBuildingSetEvent : BaseEvent
    {
    }

    public class ConcreteBuildingSetArgs : EventArgs
    {
        public ConcreteBuildingObject ConcreteBuildingObject;

        public ConcreteBuildingSetArgs(ConcreteBuildingObject obj)
        {
            ConcreteBuildingObject = obj;
        }
    }
}