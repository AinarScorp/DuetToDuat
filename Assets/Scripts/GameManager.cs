using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Inputs;
using MainGame.UI;
using UnityEngine;

//Made By Einar Hallik

namespace MainGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        GameManagerInputs gameManagerInputs;
        GameAnimations gameAnimations;
        [SerializeField] GameObject inGameCanvases;
        
        bool gameIsPaused;
        
        public event Action OnGamePaused;
        public event Action OnGameUnPaused;
        public event Action OnStartCinematics;
        public event Action OnFinishCinematics;

        public bool GameIsPaused => gameIsPaused;

        void Awake()
        {
            gameManagerInputs = new GameManagerInputs();
            gameAnimations = FindObjectOfType<GameAnimations>();
        }
        

        void OnEnable()
        {
            gameManagerInputs.Enable();
        }

        void OnDisable()
        {
            gameManagerInputs.Disable();
        }

        void Start()
        {
            gameManagerInputs.GameManager.Pause.performed += ctx => TogglePauseGame();
            if (inGameCanvases != null)
            {
                inGameCanvases.SetActive(true);
            }
        }

        public void ToggleCinematics(bool startCinamatics)
        {
            if (startCinamatics)
            {
                OnStartCinematics?.Invoke();
            }
            else
            {
                OnFinishCinematics?.Invoke();
            }
        }
        public void TogglePauseGame()
        {
            if (gameIsPaused)
            {
                Time.timeScale = 1f;
                AudioListener.pause = false;
                OnGameUnPaused?.Invoke();
            }
            else
            {
                Time.timeScale = 0f;
                AudioListener.pause = true;
                OnGamePaused?.Invoke();
            }
            gameIsPaused = !gameIsPaused;
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1f;

            gameAnimations.TriggerTransitionToMainMenu();
            
        }

    }
    
}
