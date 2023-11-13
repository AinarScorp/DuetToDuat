using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Managers;
using UnityEngine;

//Made By Einar Hallik

namespace MainGame.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] Transform pauseMenuStartWindow;
        [SerializeField] Transform controlsWindow;
        GameManager gameManager;
        
        
        void Awake()
        {
            gameManager = GameObject.FindWithTag("GameManager")?.GetComponent<GameManager>();
        }
        void Start()
        {
            gameManager.OnGamePaused += () => SetEnabled(true);
            gameManager.OnGameUnPaused +=() =>
            {
                SetEnabled(false);
                ToggleControls(false);
            };
            ToggleControls(false);
            SetEnabled(false);
        }

        public void ResumeGame()
        {
            gameManager.TogglePauseGame();
        }

        public void ToggleControls(bool turnOn)
        {
            if (controlsWindow == null) return;
            if (pauseMenuStartWindow == null) return;
            pauseMenuStartWindow.gameObject.SetActive(!turnOn);
            controlsWindow.gameObject.SetActive(turnOn);
        }
        public void GoToMainMenu()
        {
            gameManager.GoToMainMenu();
            
        }

        void SetEnabled(bool turnOn)
        {
            this.gameObject.SetActive(turnOn);

        }
        
    }
}
