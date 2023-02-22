using System;
using UnityEngine;

namespace RCRCoreLib.Core.AI.TaskSystem.Tasks
{
    public class BoatTask : TaskBase
    {
        public class BoatMoveToDock: BoatTask
        {
            //TODO implement this
        }
        
        public class BoatMoveToEnd : BoatTask
        {
            public Vector3 endPoint;
            public Action onReached;

            public BoatMoveToEnd(Vector3 endpoint, Action onEnd)
            {
                this.endPoint = endpoint;
                this.onReached = onEnd;
            }
        }
    }
}