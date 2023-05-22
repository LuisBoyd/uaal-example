using Core3.MonoBehaviors;
using CustomUIFramework.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomUIFramework.Technical
{
    public class NavigationManager : Singelton<NavigationManager>
    {
        //should handle listening to events.
        [SerializeField] [Required] 
        private NavigationEventChannelSO navigationListener;

        private void ChangeView(ViewConfig config)
        {
            
        }
    }
}