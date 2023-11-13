using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Locks;
using UnityEngine;

//Made By Einar Hallik
namespace MainGame.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        [SerializeField] CinematicsEnum cinematicToTrigger = CinematicsEnum.NotSelected;
        LockBase movingLockPuzzleCompleted;

        
        void Awake()
        {
            movingLockPuzzleCompleted = GetComponent<LockBase>();
            if (movingLockPuzzleCompleted == null)
            {
                Debug.LogError($"I deleted myself, so no cinematic on {this.gameObject.name} object");
                Destroy(this);
            }
        }
        void Start()
        {
            movingLockPuzzleCompleted.OnLockActivate += PlayCinematic;
        }
        void PlayCinematic()
        {
            FindObjectOfType<CinematicsManager>()?.PlayCutscene(cinematicToTrigger);
            Destroy(this);
        }
    }
}
