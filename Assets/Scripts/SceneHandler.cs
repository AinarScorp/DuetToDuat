using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

//Made by Einar Hallik
namespace MainGame.SceneHandler
{
    public class SceneHandler : MonoBehaviour
    {
        const string FADEOUT_TRIGGER = "FadeOut";
        const string FADEOUT_TRIGGER_SCENE_CHANGE = "FadeOutSceneChange";
        const string NEXT_SCENE_INDEX_PARAMETER = "NextSceneIndex";

        [SerializeField] int defaultNextSceneIndex;
        [SerializeField] float quitGameDelay = 1;

        [SerializeField] Animator transitionHandler;

        public void StartFadeOut()
        {
            StartFadeOut(defaultNextSceneIndex);
        }

        public void StartFadeOut(int sceneIndex)
        {
            if (transitionHandler == null)
            {
                return;
            }
            transitionHandler.SetInteger(NEXT_SCENE_INDEX_PARAMETER, sceneIndex);
            transitionHandler.SetTrigger(FADEOUT_TRIGGER_SCENE_CHANGE);
        }

        public void FadeOutFinished(int sceneIndex)
        {
            if (sceneIndex > SceneManager.sceneCountInBuildSettings)
            {
                return;
            }
            SceneManager.LoadScene(sceneIndex);
        }


        public void QuitGame()
        {
            if (transitionHandler != null)
            {
                transitionHandler.SetTrigger(FADEOUT_TRIGGER);
            }

            StartCoroutine(QuitDelay());
        }

        IEnumerator QuitDelay()
        {
            yield return new WaitForSeconds(quitGameDelay);
            Application.Quit();

        }
    }
}