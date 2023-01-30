using System;
using NewScripts.Model;
using UnityEngine;

namespace Events.Library.Models.WorldEvents
{
    public class WorldBoundsChanged : BaseEvent
    {
    }

    public class WorldBoundsChangedArgs : EventArgs
    {
        public Vector2[] Points;
        public bool SnapCameraToPoint;
        public Vector2 CameraSnapPoint;

        public WorldBoundsChangedArgs(Vector2[] points, bool SnapCameraToPoint = false, Vector2 cameraSnapPoint = default)
        {
            Points = points;
            this.SnapCameraToPoint = SnapCameraToPoint;
            CameraSnapPoint = cameraSnapPoint;

        }
    }
}