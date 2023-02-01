using RCR.Settings.NewScripts.Tilesets;
using UnityEngine;

namespace RCR.Settings.NewScripts.TaskSystem
{
    public class Task: TaskSystem.TaskBase
    {
        #region Sub-Classes
            
        public class MoveToPosition : Task
        {
            public Vector3 targetPosition;
        }
            
        public class PlayAnimation : Task
        {
                
        }
            
        #endregion
    }

    public class BoatTask : TaskBase
    {
        #region sub-Classes
        
        public class BoatMoveToDock : BoatTask
        {
            //Later on Implement A dock Structure that holds the Tile place
           
        }
        
        public class BoatMoveToEnd : BoatTask
        {
            //May want to wrap this tile up rather than having it exposed
            
        }
        
        #endregion
    }

    public class CustomerTask : TaskBase
    {
        #region sub-Classes

        

        #endregion
    }
}