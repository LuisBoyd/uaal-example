using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Building
{
    public abstract class BuildingPredicate : MonoBehaviour
    {
        public abstract bool CanBuildHere(BoundsInt area);
    }
}