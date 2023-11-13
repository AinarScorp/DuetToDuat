using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using Unity.VisualScripting;
using UnityEngine;

using AudioType = MainGame.Audio.AudioType;

// Made by Max Ekberg.
namespace MainGame.Keys
{
    public class Key : MonoBehaviour
    {
        [SerializeField] protected AudioType soundClip = AudioType.None;
        
        // Bool to set if key can deactivate.
        [SerializeField] protected bool isActivationPermanent = false;
        
        
        AudioSource audioSource;
        
        // Auto-Property is here so that we can have a generic collection of Interactables leading to the same lock.
        public bool IsActivated { get; protected set; }

        public event Action OnActivated;
        public event Action OnDeactivated;
        
        
        protected virtual void Awake()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        protected virtual void Start()
        {
            OnActivated += PlaySoundClip;
            OnDeactivated += StopSoundClip;
        }

        // Virtual so that different Keys can activate using different requirements.
        protected virtual bool ActivationCheck(Collider collider)
        {
            return true;
        }

        protected void Activate()
        {
            Debug.Log("Key activated", gameObject);
            OnActivated?.Invoke();
        }

        protected void Deactivate()
        {
            Debug.Log("Key deactivated", gameObject);
            OnDeactivated?.Invoke();
        }
        
        protected void PlaySoundClip()
        {
            AudioManager.Instance?.PlayAudio(soundClip, audioSource);
        }

        protected void StopSoundClip()
        {
            AudioManager.Instance?.StopAudio(soundClip, audioSource);
        }
        
        public virtual void SetActive(bool isOn)
        {
            IsActivated = isOn;
            if (isOn)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

    }
}

