using System;
using UnityEngine;

namespace Events.Library.Models.WorldEvents
{
    public class TilemapChanged : BaseEvent
    {
        public TilemapChangedEventArgs Args;
        
        public TilemapChanged(TilemapChangedEventArgs args =
            TilemapChangedEventArgs.empty)
        {
            Args = args;
        }
    }

    public class TilemapChangedEventArgs : EventArgs
    {
        public const TilemapChangedEventArgs empty = default;

        public BoundsInt Bounds;
        public int Quantity;
        
        public TilemapChangedEventArgs(){}

        public TilemapChangedEventArgs(BoundsInt changedBounds,
            int tileQuantity)
        {
            this.Bounds = changedBounds;
            this.Quantity = tileQuantity;
        }
    }
}