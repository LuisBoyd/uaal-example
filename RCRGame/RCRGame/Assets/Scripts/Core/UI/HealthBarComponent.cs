using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RCRCoreLib.Core.UI
{
    public class HealthBarComponent : VisualElement
    {
          private static class ClassNames
       {
           
           public static string HealthBarBackground = "Level-Progress__background";
           public static string HealthBarProgress = "Level-Progress__progress";
           public static string HealthBarTitle = "Level-Progress__title";
           public static string HealthBarLabel = "Level-Progress__label";
           public static string HealthBarContainer = "Level-Progress__container";
           public static string HealthBarTitleBackground = "Level-Progress__title_background";
           public static string StarBackground = "Level-Progress-bar__StarBackground";
           public static string CurrentLevelText = "Level-Progress-bar__LevelText";
       }

       public new class UxmlFactory : UxmlFactory<HealthBarComponent, UxmlTraits>
       {
       }

       public new class UxmlTraits : VisualElement.UxmlTraits
       {
           readonly UxmlIntAttributeDescription _currentHealth = new UxmlIntAttributeDescription
               {name = "currentHealth", defaultValue = 0};

           readonly UxmlIntAttributeDescription _mininumHealth = new UxmlIntAttributeDescription
               {name = "MinimumHealth", defaultValue = 0};

           readonly UxmlIntAttributeDescription _maximumHealth = new UxmlIntAttributeDescription
               {name = "MaximumHealth", defaultValue = 100};

           private readonly UxmlIntAttributeDescription _currentLevel = new UxmlIntAttributeDescription()
           {
               name = "CurrentLevel", defaultValue = 1
           };

          

           public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
           {
               base.Init(ve, bag, cc);
               var healthBar = (HealthBarComponent) ve;
               healthBar.CurrentHealth = _currentHealth.GetValueFromBag(bag, cc);
               healthBar.MinimumHealth = _mininumHealth.GetValueFromBag(bag, cc);
               healthBar.MaximumHealth = _maximumHealth.GetValueFromBag(bag, cc);
               healthBar.CurrentLevel = _currentLevel.GetValueFromBag(bag, cc);
           }
       }
       //Must have properties that are named the same as the defined Traits in order for data to be properly
       //linked and displayed in the Builder 
       private int _currentHealth;
       private int _minimumHealth;
       private int _maximumHealth;
       private int _currentLevel;
       private readonly Label _currentLevelLabel;
       private VisualElement _progress;
       private VisualElement _background;
       private VisualElement _StarBackground;

       public int CurrentHealth
       {
           get => _currentHealth;
           set
           {
               if (value == _currentHealth)
                   return;
               _currentHealth = value;
               SetHealth(_currentHealth, _maximumHealth);
           }
       }
       public int MinimumHealth
       {
           get => _minimumHealth;
           set => _minimumHealth = value;
       }
       public int MaximumHealth
       {
           get => _maximumHealth;
           set
           {
               if (value == _maximumHealth)
                   return;
               _maximumHealth = value;
               SetHealth(_currentHealth, _maximumHealth);
           }
       }

       public int CurrentLevel
       {
           get => _currentLevel;
           set
           {
               if(value == _currentLevel)
                   return;
               _currentLevel = value;
               SetLevel(_currentLevel);
           }
       }



       public HealthBarComponent()
       {
           AddToClassList(ClassNames.HealthBarContainer);
           //Add Elements and their class selectors to the Component.
           _background = new VisualElement {name = "HealthBarBackground"};
           _background.AddToClassList(ClassNames.HealthBarBackground);
           Add(_background);

           _progress = new VisualElement {name = "HealthBarProgress"};
           _progress.AddToClassList(ClassNames.HealthBarProgress);
           _background.Add(_progress);
           
           // _StarBackground = new VisualElement {name = "LevelDisplayBackground"};
           // _StarBackground.AddToClassList(ClassNames.StarBackground);
           // _background.Add(_StarBackground);
           //
           // _currentLevelLabel = new Label() {name = "LevelStat"};
           // _currentLevelLabel.AddToClassList(ClassNames.CurrentLevelText);
           // _StarBackground.Add(_currentLevelLabel);
           
       }

       private void SetHealth(int currentHealth, int maxHealth)
       {
           if (maxHealth > 0)
           {
               float w = Mathf.Clamp((float) currentHealth / maxHealth * 100, 0f, 100f);
               _progress.style.width = new StyleLength(Length.Percent(w));
           }
       }

       private void SetLevel(int currentLevel)
       {
           //_currentLevelLabel.text = currentLevel.ToString();
       }
       

      
    }
}
