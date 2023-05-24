using System.Collections.Generic;
using System.Linq;
using deVoid.UIFramework.Extends.Premade;
using deVoid.UIFramework.Extends.Premade.Data;
using deVoid.UIFramework.Extends.Premade.Events;
using UnityEngine;

namespace deVoid.UIFramework.Extends.ProjectSpecific.Controllers
{
    public class GameMainPanelController : APanelController
    {
        [SerializeField] private List<NavigationPanelEntry> _navigationTargets = new List<NavigationPanelEntry>();

        private readonly List<NavigationPanelButton> currentButtons = new List<NavigationPanelButton>();
        
        //TODO Here set up a Dictionary of panelButton + NavPanel Entry

        [SerializeField] private NavigateToWindowEventSO _navigateToWindowSignal;

        protected override void AddListeners()
        {
            _navigateToWindowSignal.onEventRaised += OnExternalNavigation;
        }

        protected override void RemoveListeners()
        {
            _navigateToWindowSignal.onEventRaised -= OnExternalNavigation;
        }

        protected override void OnPropertiesSet()
        {
            ClearEntries();
            //TODO here reassign the data to the appropriate target as well as hook up the event.
            GetComponentsInChildren<NavigationPanelButton>().ToList().ForEach(NavButton =>
            {
                NavButton.SetData(_navigationTargets.First());
                NavButton.ButtonClicked += OnNavigationButtonClicked;
                currentButtons.Add(NavButton);
            });
        }

        private void OnExternalNavigation(string screenID)
        {
            foreach (NavigationPanelButton currentButton in currentButtons)
            {
                currentButton.SetCurrentNavigationTarget(screenID);
            }
        }

        private void ClearEntries()
        {
            foreach (NavigationPanelButton currentButton in currentButtons)
            {
                currentButton.ButtonClicked -= OnNavigationButtonClicked;
                Destroy(currentButton.gameObject); //TODO optimization possible by using Pooling Pattern
                //TODO Implement the pool pattern if possible if not Disable the GameObjects.
            }
        }

        private void OnNavigationButtonClicked(NavigationPanelButton navigationPanelButton)
        {
            _navigateToWindowSignal.RaiseEvent(navigationPanelButton.Target);
            foreach (NavigationPanelButton currentButton in currentButtons)
            {
                currentButton.SetCurrentNavigationTarget(navigationPanelButton);
            }
        }
    }
}