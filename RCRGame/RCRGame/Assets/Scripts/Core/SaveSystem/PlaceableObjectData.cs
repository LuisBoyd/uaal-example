using System;
using UnityEngine;

namespace RCRCoreLib.Core.SaveSystem
{
    [Serializable]
    public class PlaceableObjectData : Data
    {
        public string assetName;
        public Vector3 position;
    }
}