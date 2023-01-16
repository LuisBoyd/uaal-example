using System;
using DataStructures;
using RCR.Settings;
using RCR.Utilities;
using UnityEngine;

namespace BuildingComponents.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Building Scriptable Object", menuName = "RiverCanalRescue/Buildings/Building-Object", order = 0)]
    public class ConcreteBuildingObject : ScriptableObject
    {
        public PrestigeXBuildingObject[] PrestigeXBuildingObjects;
        public TypeOfBuilding TypeOfBuilding;
        private void OnValidate()
        {
            if (LBUtilities.AssertNull(PrestigeXBuildingObjects))
            {
                if(PrestigeXBuildingObjects.Length > GameSettings.Max_Prestige)
                    Array.Resize(ref PrestigeXBuildingObjects, GameSettings.Max_Prestige);
            }
        }
    }
}