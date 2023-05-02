using Sirenix.OdinInspector;
using UI.UIArchitecture;
using UnityEngine;

namespace Core.Optimisation.Patterns.Factory
{
    public class GameUIButtonFactory : Factory<GameHUDButton>
    {
        [Required] [SerializeField] private GameHUDButton preset;
        public override GameHUDButton Create()
        {
            return Instantiate(preset);
        }
    }
}