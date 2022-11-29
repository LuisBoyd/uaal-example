using System;
using System.Collections.Generic;
using RCR.Enums;
using RCR.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using PlayerInputComponent = UnityEngine.InputSystem.PlayerInput;
namespace RCR.Input
{
    [RequireComponent(typeof(PlayerInputComponent))]
    public class CustomInputModule : MonoBehaviour
    {
        private PlayerInputComponent m_playerInputComponent;
        private InputActionAsset m_playerInput;

        private InputActionMap m_currentMap;
        
        
        
        private void Awake()
        {
            m_playerInputComponent = GetComponent<PlayerInputComponent>();
            m_playerInput = m_playerInputComponent.actions;
            
        }

        private void AssignFunctionality()
        {
            m_playerInput.FindAction("OnScreenTap", true).performed += OnScreenTap;
        }

      
        private void OnScreenTap(InputAction.CallbackContext obj)
        {
            //TODO Implement method
        }

        private void OnGameModeSwitch(GameMode mode)
        {
            m_playerInput.Disable();
            Debug.Log($"GameMode: {mode}, Switching Map");
            switch (mode)
            {
                case GameMode.GamePlay:
                   m_currentMap = m_playerInput.FindActionMap("Gameplay", true);
                    break;
                case GameMode.UI:
                    m_currentMap = m_playerInput.FindActionMap("UI", true);
                    break;
            }
            m_currentMap.Enable();
        }
        
        


        private void OnEnable()
        {
            GameManager.Instance.OnGameModeSwitch += OnGameModeSwitch;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameModeSwitch -= OnGameModeSwitch;
        }
    }
}