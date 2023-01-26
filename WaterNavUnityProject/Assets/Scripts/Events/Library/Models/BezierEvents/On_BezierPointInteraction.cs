using System;
using Bezier;

namespace Events.Library.Models.BezierEvents
{
    public class On_BezierPointInteraction : BaseEvent
    {
        
        public On_BezierPointInteraction()
        {
            
        }
    }

    public class On_BezierPointInteractionArgs : EventArgs
    {
        public BezierPointState CurrentState;
        
        public On_BezierPointInteractionArgs(BezierPointState state)
        {
            CurrentState = state;
        }
    }
    
    
}