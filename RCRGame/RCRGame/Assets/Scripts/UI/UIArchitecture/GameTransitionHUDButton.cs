using DefaultNamespace.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIArchitecture
{
    public class GameTransitionHUDButton : GameHUDButton
    {
        [Title("Game Transition HUD Button Configurations", TitleAlignment = TitleAlignments.Centered)] 
        [SerializeField][Required] private UIDisplayEvent TargetScreenEventChannel;

        private string _screenTarget;
        private bool _clearWindowOnPress;
        
        public override void SetData(GameHUDButtonEntry buttonEntry)
        {
            base.SetData(buttonEntry);
            _screenTarget = buttonEntry.TargetScreen;
            _clearWindowOnPress = buttonEntry.ClearWindowOnPress;
        }

        public void SetTarget(string target, bool clearWindowOnPress = false)
        {
            _screenTarget = target;
            _clearWindowOnPress = clearWindowOnPress;
        }

        protected override void UI_Click()
        {
            if (TargetScreenEventChannel != null)
            {
                TargetScreenEventChannel.RaiseEvent(_clearWindowOnPress,_screenTarget);
            }
        }
    }
}