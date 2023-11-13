using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Cinematics;
using MainGame.Inputs;
using MainGame.SceneHandler;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

//Made by Einar Hallik
namespace MainMenu.UI
{
    public class PlayerConnectionSection : MainMenuSection
    {
        const string FADEOUT_TRIGGER = "FadeOut";

        [SerializeField] SplitScreenManager playerConnector;
        [SerializeField] Animator transitionAnimator;
        
        void Start()
        {
            PlayerInputManager playerInputManager = playerConnector.GetComponent<PlayerInputManager>();
            playerInputManager.onPlayerJoined += ReactToPlayerJoined;

        }

        void ReactToPlayerJoined(PlayerInput playerInput)
        {
            CinematicsManager cinematicsManager = FindObjectOfType<CinematicsManager>();
            //First player connected
            if (playerInput.playerIndex == 0 && cinematicsManager!= null)
            {
                cinematicsManager.PlayCutscene(CinematicsEnum.PlayerConnectionFlyToSpirit);
            }
            //Second player connected
            else if (playerInput.playerIndex == 1 && transitionAnimator !=null)
            {
                FindObjectOfType<MainMenuController>()?.ToggleSection(Section.IntroSection, true);
            }
        }

    }
}
