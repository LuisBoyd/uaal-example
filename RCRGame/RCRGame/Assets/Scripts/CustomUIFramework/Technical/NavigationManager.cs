using Core3.MonoBehaviors;
using CustomUIFramework.Event;
using CustomUIFramework.Organisms;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomUIFramework.Technical
{
    public class NavigationManager : Singelton<NavigationManager>
    {
        [SerializeField] [Required] 
        private SliceManager _sliceManager;
        //should handle listening to events.
        [SerializeField] [Required] 
        private NavigationEventChannelSO navigationListener;
        public NavigationEventChannelSO NavigationChannel
        {
            get => navigationListener;
        }
        

        private ViewConfig _currentManagedViewConfig;

        private void ChangeView(ViewConfig config)
        {
            if (config.HidePrevious && _currentManagedViewConfig != null)
            {
                //ok so I need to make the Ui slide to the left in response
                //
            }
        }

        private void HideCurrentView(ViewConfig config)
        {
            var slices = _sliceManager.GatherSlices(config);
            foreach (Slice slice in slices)
            {
                //config.HideTransitionConfig.ShowTransition(slice.);
            }
        }
    }
}