using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Cinematics;
using UnityEngine;

//Made by Einar Hallik
namespace MainMenu.UI
{
    public class MainMenuController : MonoBehaviour
    {
        const string FADEOUT_TRIGGER = "FadeOut";
        const string FADEIN_TRIGGER = "FadeIn";
        
        [SerializeField] Animator transitionHandler;
        [SerializeField] float fadeInDuration = 1;
        MainMenuSection[] mainMenuSections;
        List<MainMenuSection> activeSections = new List<MainMenuSection>();
        CinematicsManager cinematicsManager;

        void Awake()
        {
            mainMenuSections = Resources.FindObjectsOfTypeAll<MainMenuSection>();
            cinematicsManager = FindObjectOfType<CinematicsManager>();
        }

        void Start()
        {
            DisableAllSections();
            TurnOnNewSection(Section.Start);
        }

        void DisableAllSections()
        {
            foreach (var mainMenuSection in mainMenuSections)
            {
                mainMenuSection.SetEnabled(false);
            }
        }

        IEnumerator FadeInFadeOut(Section nextSection)
        {
            if (transitionHandler == null)
            {
                yield break;
            }
            TurnOffActiveSections();
            transitionHandler.SetTrigger(FADEOUT_TRIGGER);
            yield return new WaitForSeconds(fadeInDuration);
            TurnOnNewSection(nextSection);
            transitionHandler.SetTrigger(FADEIN_TRIGGER);
        }
        public void ToggleSection(Section nextSection, bool useFadeOut = false)
        {
            if (useFadeOut)
            {
                StartCoroutine(FadeInFadeOut(nextSection));
            }
            else
            {
                TurnOffActiveSections();
                TurnOnNewSection(nextSection);
            }
        }
        public void ToggleSection(SectionButton sectionButton)
        {
            ToggleSection(sectionButton.SectionToActivate);
            if (sectionButton.Cinematic != CinematicsEnum.NotSelected && cinematicsManager != null)
            {
                cinematicsManager.PlayCutscene(sectionButton.Cinematic);
            }
        }
        
        
        void TurnOnNewSection(Section newSection)
        {
            foreach (var mainMenuSection in mainMenuSections)
            {
                if (mainMenuSection.Section == newSection)
                {
                    mainMenuSection.SetEnabled(true);
                    activeSections.Add(mainMenuSection);
                }
            }
        }

        void TurnOffActiveSections()
        {
            foreach (MainMenuSection activeSection in activeSections)
            {
                activeSection.SetEnabled(false);
            }
            activeSections.Clear();
        }
    }
}