using System.Collections.Generic;
using deVoid.UIFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIArchitecture
{
    public class MenuPanelController : APanelController
    {
        [Title("Menu Panel Controller Configurations", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField]
        private List<GameHUDButtonEntry> ConstructionOptionEntries = new List<GameHUDButtonEntry>();
        [Required] [SerializeField] private GameHUDButton templateButton;
        [Required] [SerializeField] private GameTransitionHUDButton transitionTemplateButton;

        private readonly List<GameHUDButton> currentButtons = new List<GameHUDButton>();

        protected override void OnPropertiesSet()
        {
            foreach (GameHUDButtonEntry entry in ConstructionOptionEntries)
            {
                if (string.IsNullOrEmpty(entry.TargetScreen))
                {
                    //Instnatiate the transition screen
                    var newHudButton = Instantiate(transitionTemplateButton);
                    newHudButton.transform.SetParent(templateButton.transform.parent,false);
                    newHudButton.SetData(entry);
                    newHudButton.gameObject.SetActive(true);
                    currentButtons.Add(newHudButton);
                    
                }
                else
                {
                    var newHudButton = Instantiate(templateButton);
                    newHudButton.transform.SetParent(templateButton.transform.parent,false);
                    newHudButton.SetData(entry);
                    newHudButton.gameObject.SetActive(true);
                    currentButtons.Add(newHudButton);
                }
            }
        }
    }
}