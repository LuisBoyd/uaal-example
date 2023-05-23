using System;
using Core3.MonoBehaviors;
using DefaultNamespace.Events;
using deVoid.UIFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.App
{
    public class GameUIController : Singelton<GameUIController>
    {
        [SerializeField] [Required] 
        private UISettings defaultUISettings;

        private UIFrame _uiFrame;

        #region MainWindow
        [BoxGroup("Main Window")]
        [BoxGroup("Main Window/Listeners")]
        [Required] [SerializeField]
        private EventRelay StartGameSignal; //the start Game signal is called after the we have loaded into the scene 
        [BoxGroup("Main Window/Listeners")]
        [Required] [SerializeField]
        private EventRelay OpenShoppingSignal; //When the user has clicked the shopping menu window
        [BoxGroup("Main Window/Listeners")]
        [Required] [SerializeField]
        private EventRelay OpenTileModificationSignal; //when the user has clicked the tile modification button.
        [BoxGroup("Main Window/Listeners")]
        [Required] [SerializeField]
        private EventRelay OpenTasksPopupSignal; //when the user clicks the tasks button. for a popup
        [BoxGroup("Main Window/Listeners")]
        [Required] [SerializeField]
        private EventRelay OpenFriendsPopupSignal; //when the user clicks the friends popup button.
        [BoxGroup("Main Window/Listeners")]
        [Required] [SerializeField]
        private EventRelay OpenCardsRewardsSignal; //when the user clicks the Cards reward button.
        [BoxGroup("Main Window/Listeners")]
        [Required] [SerializeField]
        private EventRelay SettingsPopupSignal; //when the user clicks the settings button.
        [BoxGroup("Main Window/Listeners")]
        [Required] [SerializeField]
        private EventRelay PurchasePopupSignal; //when the user either clicks the + on premium currency or standard currency

        private void RegisterMainWindow()
        {
            
        }
        private void DeRegisterMainWindow()
        {
            
        }
        #endregion
        private void Awake()
        {
            _uiFrame = defaultUISettings.CreateUIInstance();
            RegisterMainWindow();
        }
        

        private void OnDestroy()
        {
            DeRegisterMainWindow();
        }

        private void Start()
        {
            //Show the main screen done by panels.
            _uiFrame.ShowPanel(GameScreenIds.StandardGameWindowHUD);
            _uiFrame.ShowPanel(GameScreenIds.StandardGameWindowBtns);
        }

        private void On_TileBtnClick()
        {
            //Hide These
            _uiFrame.HidePanel(GameScreenIds.StandardGameWindowBtns);
            //Show These
            _uiFrame.ShowPanel(GameScreenIds.TileModificationWindowBtns);
        }

        private void On_ShoppingBtnClick()
        {
            //Hide These
            _uiFrame.HidePanel(GameScreenIds.StandardGameWindowBtns);
            //Show These
            _uiFrame.ShowPanel(GameScreenIds.ShoppingCategoryBtns); //TODO replace this with a shoppingManager and pass it the uiframe to show the panels needed
            _uiFrame.ShowPanel(GameScreenIds.AttractionsCategoryBtns);
        }

        private void On_FriendsBtnClick()
        {
            //pop-up this
            _uiFrame.ShowPanel(GameScreenIds.FriendsPopup);
        }
    }
}