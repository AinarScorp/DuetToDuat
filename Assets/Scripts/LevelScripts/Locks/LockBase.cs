using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Keys;

using AudioType = MainGame.Audio.AudioType;

// Made by Max Ekberg.
namespace MainGame.Locks
{
    public class LockBase : MonoBehaviour
    {
        [SerializeField] protected Key[] keys;
    
        [SerializeField] protected AudioType soundClip = AudioType.None;
        
        [SerializeField] protected bool isActivationPermanent = false;
        
        [SerializeField] bool showConnections = false;
        
        protected bool isActive = false;

        protected AudioSource audioSource;

        public event Action OnLockActivate;
        public event Action OnLockDeactivate;
        
        protected virtual void Awake()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        protected virtual void Start()
        {
            foreach (var key in keys)
            {
                key.OnActivated += KeyCheck;
                key.OnDeactivated += KeyCheck;
            }
            
            OnLockActivate += HandleOnState;
            OnLockDeactivate += HandleOffState;
        }

        // Checks for if all the keys needed for the door to open are active.
        protected virtual void KeyCheck()
        {
            isActive = true;
            foreach (var key in keys)
            {
                if (!key.IsActivated)
                {
                    Debug.Log(key.IsActivated);
                    isActive = false;
                }
            }
            
            Debug.Log($"Lock state is {isActive}");
            
            StateCheck();
        }
        
        //  Handles calling of disabling and enabling of lock.
         protected virtual void StateCheck()
         {
             if (isActive)
             {
                 OnLockActivate?.Invoke();
             }
             else if(!isActivationPermanent)
             {
                 OnLockDeactivate?.Invoke();
             }
         }
    
        //TODO Maybe make into interface methods.
        protected virtual void HandleOnState()
        {
            Debug.Log("Lock activated", gameObject);
        }

        protected virtual void HandleOffState()
        {
            Debug.Log("Lock deactivated", gameObject);
        }
        
    
    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!showConnections) return;
            
            foreach (var key in keys)
            {
                if (key== null)
                {
                    Debug.LogWarning("There are not keys in the slot", gameObject);
                    continue;
                }
                Gizmos.color = key.IsActivated ? Color.green : Color.red;
                Gizmos.DrawLine(key.transform.position, transform.position);
            }
        }
    #endif
    }
}
