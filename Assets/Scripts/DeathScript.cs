using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using MainGame.Movement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.VFX;
using AudioType = MainGame.Audio.AudioType;

// Made by Max Ekberg.
namespace MainGame.Death
{
    public class DeathScript : MonoBehaviour
    {
        [SerializeField] GameObject meshObject;
        
        [SerializeField] VisualEffect deathFx;
        
        [SerializeField] Transform spawnTransform;
    
        string tagToCheck;

        Coroutine deathCoroutine;

        AudioSource audioSource;
        public event Action OnPlayerDied;
        public event Action OnPlayerRevive;

    
        void Awake()
        {
            audioSource = GetComponentInChildren<AudioSource>();
            
            deathFx.Stop();
            
            tagToCheck = this.CompareTag("BodyPlayer") ? "VesselDeathTrigger" : "SpiritDeathTrigger";
        }

        IEnumerator RespawnTimer()
        {
            AudioType soundToPlay = tagToCheck == "VesselDeathTrigger" ? AudioType.BodyDeath : AudioType.SpiritDeath;
            AudioManager.Instance?.PlayAudio(soundToPlay, audioSource);

            deathFx.Play();
            meshObject.SetActive(false);
            
            yield return new WaitForSeconds(3f);
            transform.position = spawnTransform.position;

            meshObject.SetActive(true);
            OnPlayerRevive?.Invoke();
            
            deathCoroutine = null;
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (tagToCheck == "VesselDeathTrigger" && other.CompareTag("CheckpointTrigger"))
            {
                spawnTransform = other.transform;
                other.enabled = false;
            }
        
            if (!other.CompareTag(tagToCheck) && !other.CompareTag("DeathTrigger") || deathCoroutine != null) return;

            OnPlayerDied?.Invoke();
            
            deathCoroutine = StartCoroutine(RespawnTimer());
        }

        
    }
}
