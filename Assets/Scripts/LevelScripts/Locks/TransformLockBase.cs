using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using UnityEngine;

// Made by Max Ekberg.
namespace MainGame.Locks
{
    public class TransformLockBase : LockBase
    {
        
        [SerializeField] AnimationCurve transformSpeedCurve = AnimationCurve.Linear(0,0,1,1);
    
        protected Coroutine transformCoroutine;

        private Vector3 currentTransformValue;
        
        protected Vector3 CurrentTransformValue
        {
            get => currentTransformValue;
            private set
            {
                currentTransformValue = value;
                OnTransformValueChanged?.Invoke();
            }
        }
        
        public event Action OnTransformValueChanged;
        
        
        protected void StopMoveRoutine(Coroutine passedCoroutine)
        {
            if (passedCoroutine == null) return;
            
            StopCoroutine(passedCoroutine);
        }
        
        protected IEnumerator TransformLock(Vector3 startPos, Vector3 endPos)
        {
            CurrentTransformValue = startPos;
            
            AudioManager.Instance?.PlayAudio(soundClip, audioSource);
            
            float t = 0f;
            var lastFrame = transformSpeedCurve[transformSpeedCurve.length - 1];
    
            while (t < lastFrame.time)
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
                CurrentTransformValue = Vector3.Lerp(startPos, endPos, transformSpeedCurve.Evaluate(t));
            }

            CurrentTransformValue = endPos;

            AudioManager.Instance?.StopAudio(soundClip, audioSource);
            
            transformCoroutine = null;
        }
    }
}
