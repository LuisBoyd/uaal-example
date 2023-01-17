using DataStructures;
using UnityEngine;

namespace BuildingComponents.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Prestige_X_BuildingType", menuName = "RiverCanalRescue/Buildings/Prestige-Object", order = 0)]
    public class PrestigeXBuildingObject : ScriptableObject
    {
        [SerializeField]
        private Texture2D prestigeTexture;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private UnityDateTime buildingTimeForThis;
        [SerializeField]
        private Texture2D[] buildingAnimation;

        public string Name;
        public int Cost;
        [TextArea] 
        public string Description;


        public Animator Animator
        {
            get => animator;
        }
        public Texture2D PrestigeTexture
        {
            get => prestigeTexture;
        }
        public UnityDateTime BuildingTimeForThis
        {
            get => buildingTimeForThis;
        }
        public Texture2D[] BuildingAnimation
        {
            get => buildingAnimation;
        }
    }
}