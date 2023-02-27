using UnityEngine;

namespace RCRCoreLib.Core.Animation
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "GameObjects/Isometric Animation Data", order = 0)]
    public class IsometricAnimatorData : ScriptableObject
    {
        [SerializeField] 
        private RuntimeAnimatorController m_runtimeAnimatorController;
        public RuntimeAnimatorController RuntimeAnimatorController
        {
            get => m_runtimeAnimatorController;
        }
        
    }
}