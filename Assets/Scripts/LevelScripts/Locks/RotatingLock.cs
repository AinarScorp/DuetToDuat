using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Made by Max Ekberg.
namespace MainGame.Locks
{
    public class RotatingLock : TransformLockBase
    {
        [SerializeField] Vector3 openRotDeltaAngles;
        
        Vector3 openRot;
        Vector3 closedRot;
        
        protected override void Awake()
        {
            base.Awake();
            
            closedRot = transform.rotation.eulerAngles;
            openRot = closedRot + openRotDeltaAngles;
        }
    
        protected override void Start()
        {
            base.Start();

            OnTransformValueChanged += () => { transform.rotation = Quaternion.Euler(CurrentTransformValue); };
        }
        
        protected override void HandleOnState()
        {
            isActive = true;

            StopMoveRoutine(transformCoroutine);
            
            var currentAngles = transform.rotation.eulerAngles;
            transformCoroutine = StartCoroutine(TransformLock(currentAngles, openRot));
        }
    
        protected override void HandleOffState()
        {
            isActive = false;

            StopMoveRoutine(transformCoroutine);
            
            var currentAngles = transform.rotation.eulerAngles;
            
            transformCoroutine = StartCoroutine(TransformLock(closedRot, currentAngles));
        }
    }
}
