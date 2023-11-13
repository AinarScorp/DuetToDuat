using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Inputs;
using MainGame.SceneHandler;
using UnityEngine;
using UnityEngine.Video;

//Made By Einar Hallik
namespace MainMenu.UI
{
    public class IntroSection : MainMenuSection
    {
        [SerializeField] VideoPlayer videoPlayer;
        GameManagerInputs gameManagerInputs;
        
        
        void Awake()
        {
            if (videoPlayer == null)
            {
                Destroy(this);
            }

            gameManagerInputs = new GameManagerInputs();
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
            gameManagerInputs.GameManager.SkipIntro.performed += _ => SkipIntro();
        }

        
        public override void SetEnabled(bool turnOn)
        {
            base.SetEnabled(turnOn);
            videoPlayer.gameObject.SetActive(turnOn);
            if (turnOn == false)
            {
                return;
            }
            videoPlayer.Play();
            StartCoroutine(StartGameWhenIntroIsFinished());
        }
        
        IEnumerator StartGameWhenIntroIsFinished()
        {
            yield return new WaitForSeconds(2);
            while (videoPlayer.isPlaying)
            {
                yield return null;
            }
            FindObjectOfType<SceneHandler>()?.StartFadeOut();
        }

        void SkipIntro()
        {
            videoPlayer.Stop();
        }
    }
    
}
