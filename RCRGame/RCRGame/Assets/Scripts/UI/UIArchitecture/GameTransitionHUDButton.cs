using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIArchitecture
{
    public class GameTransitionHUDButton : GameHUDButton
    {
        [Title("Game Transition HUD Button Configurations", TitleAlignment = TitleAlignments.Centered)] 
        [SerializeField][Required] private StringEventChannelSO TargetScreenEventChannel;

        private string _screenTarget;
        
        public override void SetData(GameHUDButtonEntry buttonEntry)
        {
            base.SetData(buttonEntry);
            _screenTarget = buttonEntry.TargetScreen;
        }

        protected override void UI_Click()
        {
            if (TargetScreenEventChannel != null)
            {
                TargetScreenEventChannel.RaiseEvent(_screenTarget);
            }
        }
    }
}