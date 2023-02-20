using System;

namespace RCRCoreLib.Core.SaveSystem
{
    public class DelayedPlaceableObjectData : PlaceableObjectData
    {
        public bool IsBuilt;
        public DateTime StartTime;
        public DateTime DateTimeFinish;
        public TimeSpan Duration;
    }
}