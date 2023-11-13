using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using MainGame.Keys;
using MainGame.Locks;
using UnityEngine;

//Made by Max Ekberg.
namespace MainGame.Locks
{
    public class AnimationLock : LockBase
    {
        Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected override void HandleOnState()
        {
            animator.SetTrigger("Activated");
            AudioManager.Instance?.PlayAudio(soundClip, audioSource);
        }
    }
}