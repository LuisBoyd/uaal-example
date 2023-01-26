using UnityEngine;

namespace RCR.Settings.Optimization
{
    public class WorldChunkCulling: CullingComponent
    {
        public override void ChangeInCulling(CullingGroupEvent evnt)
        {
            //
        }

        protected override void InView(CullingGroupEvent evnt)
        {
            //
        }

        protected override void OutOfView(CullingGroupEvent evnt)
        {
            //
        }
    }
}