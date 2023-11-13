using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using MainGame.Keys;
using Unity.VisualScripting;
using UnityEngine;
using AudioType = MainGame.Audio.AudioType;

// Made by Max Ekberg.
namespace MainGame.Locks
{
    public class MovingLock : TransformLockBase
    {

        [SerializeField] Key[] activationPermanentKeys;
        
        [SerializeField] Vector3 openPosDelta; // How far the open position is from the closed position.
        
        Vector3 openPos; // Position when activated.
        Vector3 closedPos; // Position when not activated.

        protected override void Awake()
        {
            base.Awake();
            closedPos = transform.localPosition;
            openPos = closedPos + openPosDelta;
        }

        protected override void Start()
        {
            base.Start();

            OnTransformValueChanged += () => { transform.localPosition = CurrentTransformValue; };
            
            OnLockActivate += PermanentActivationKeyCheck;
        }
        
        void PermanentActivationKeyCheck()
        {
            if (activationPermanentKeys.Length == 0 || isActivationPermanent) return;
            
            foreach (var key in activationPermanentKeys)
            {
                if (!key.IsActivated) return;
            }
            AudioManager.Instance.PlayAudio(AudioType.LockPlatform, audioSource);
            isActivationPermanent = true;
        }        
        
        // Sets the door to the open position.
        protected override void HandleOnState()
        {
            StopMoveRoutine(transformCoroutine);
            
            transformCoroutine = StartCoroutine(TransformLock(transform.localPosition, openPos));
        }
    
        // Sets the door to the closed position.
        protected override void HandleOffState()
        {
            StopMoveRoutine(transformCoroutine);
            
            transformCoroutine = StartCoroutine(TransformLock(transform.localPosition, closedPos));
        }
        
        
        
    }
}
