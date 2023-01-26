using UnityEngine;

namespace RCR.Settings.Optimization
{
    public interface ICulling
    {
        /// <summary>
        /// ChangeInCulling will be called whenever a change has occured in the Culling state
        /// It will work out what to call whether something is in view of the camera or not but not implement that logic
        /// </summary>
        /// <param name="evnt"></param>
        void ChangeInCulling(CullingGroupEvent evnt);
    }
}