using Sirenix.OdinInspector;
using UI.UIArchitecture;
using UnityEngine;

namespace Core.Optimisation.Patterns.Factory
{
    public class GameUITransitionButtonFactory : Factory<GameTransitionHUDButton>
    {
        [Required] [SerializeField] private GameTransitionHUDButton preset;
        
        public override GameTransitionHUDButton Create()
        {
            return Instantiate(preset);
        }
    }
}