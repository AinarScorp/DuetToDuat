using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Einar Hallik
namespace MainGame.UI
{
    public class GameAnimations : MonoBehaviour
    {
        const string OPEN_SCENE_TRIGGER = "OpenScene";
        const string CLOSE_SCENE_TRIGGER = "CloseScene";
        const string NEXT_SCENE_INDEX_PARAMETER = "NextSceneIndex";

        [SerializeField] int mainMenuSceneIndex = 0;
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            animator.SetTrigger(OPEN_SCENE_TRIGGER);
        }

        public void TriggerTransitionToMainMenu()
        {
            animator.SetInteger(NEXT_SCENE_INDEX_PARAMETER, mainMenuSceneIndex);
            animator.SetTrigger(CLOSE_SCENE_TRIGGER);
        }
    }
    
}
