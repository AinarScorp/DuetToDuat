using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;


// Made by Max Ekberg.
namespace MainGame.Keys
{
    public class BrazierKeyHandler : SpiritKey
    {
        VisualEffect fireEffect;
        
        Light brazierLight;

        protected override void Awake()
        {
            base.Awake();
            fireEffect = GetComponent<VisualEffect>();
            brazierLight = GetComponentInChildren<Light>();
            fireEffect.Stop();
            brazierLight.enabled = false;
        }

        protected override void Start()
        {
            base.Start();
            OnActivated += StartFire;
            OnDeactivated += StopFire;
        }

        // Starts the fire particles of the brazier.
        void StartFire()
        {
            fireEffect.Play();
            brazierLight.enabled = true;
        }

        void StopFire()
        {
            fireEffect.Stop();
            brazierLight.enabled = false;
        }
    }
}

