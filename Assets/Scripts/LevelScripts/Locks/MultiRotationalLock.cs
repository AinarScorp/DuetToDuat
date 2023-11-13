using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Keys;
using MainGame.Locks;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Made by Max Ekberg.
namespace MainGame.Locks
{
    public class MultiRotationalLock : TransformLockBase
    {
    
        [SerializeField] Vector3[] rotationAngles;
    
        int currentRotIndex = 0;

        protected override void Start()
        {
            base.Start();

            OnTransformValueChanged += () =>
            {
                transform.rotation = Quaternion.Euler(new Vector3(CurrentTransformValue.x, 0, 0));
            };
        }

        Vector3 GetNewRotation()
        {
            if(currentRotIndex + 1 < rotationAngles.Length)
            {
                currentRotIndex++;
                return rotationAngles[currentRotIndex];
            }
            else
            {
                currentRotIndex = 0;
                return rotationAngles[0];
            }
        }
        
        
        protected override void HandleOnState()
        {
            base.HandleOnState();
            
            var currentAngles = transform.rotation.eulerAngles;
            StopMoveRoutine(transformCoroutine);
            transformCoroutine = StartCoroutine(TransformLock(currentAngles, GetNewRotation()));
        }

        protected override void KeyCheck()
        {
            isActive = false;
            foreach (var key in keys)
            {
                if (!key.IsActivated) continue;

                isActive = true;
                break;
            }

            StateCheck();
        }

        #if UNITY_EDITOR
        void OnValidate()
        {
           rotationAngles[0] = transform.rotation.eulerAngles;
        }
        #endif
    }
}

