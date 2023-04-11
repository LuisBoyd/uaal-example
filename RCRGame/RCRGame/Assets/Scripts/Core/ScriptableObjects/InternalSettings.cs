using JetBrains.Annotations;
using UnityEngine;

namespace Core3.SciptableObjects
{
    public class InternalSetting : BaseScriptableObject
    {
        [CanBeNull] public string DBconnectionString { get; set; }
        [CanBeNull] public string RootEndPoint { get; set; }
    }
}