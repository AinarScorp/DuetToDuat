using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using Unity.VisualScripting;
using UnityEngine;

// Made by Max Ekberg.
namespace MainGame.Keys
{
    public class PhysicalKey : Key
    {
        [SerializeField] Transform decalToActivate;
        
        int amountOfActivators = 0;
    
        // Override bool to make sure only BodyPlayer and objects can activate the key.
        protected override bool ActivationCheck(Collider activator)
        {
            return activator.CompareTag("BodyPlayer") || activator.CompareTag("PhysicalKeyActivator");
        }
    
        public override void SetActive(bool isOn)
        {
            base.SetActive(isOn);
            amountOfActivators = isOn ? amountOfActivators : 0;
        }
        
        // Activates the key if the collider isn't the spirit player.
        void OnTriggerEnter(Collider other)
        {
            if (!ActivationCheck(other)) return;

            amountOfActivators++;
            
            IsActivated = amountOfActivators > 0;
            
            if (IsActivated && amountOfActivators == 1)
            {
                Activate();
            }
            
            if (decalToActivate && IsActivated)
            {
                decalToActivate.gameObject.SetActive(true);
            }
            
        }

        // Deactivates key when collider leaves if activation isn't permanent.
        void OnTriggerExit(Collider other)
        {
            if (isActivationPermanent || amountOfActivators == 0 || !ActivationCheck(other)) return;

            amountOfActivators--;
            
            IsActivated = amountOfActivators > 0;

            if (IsActivated) return;
            
            Deactivate();
            
            if (decalToActivate)
            {
                decalToActivate.gameObject.SetActive(false);
            }
        }
    }
}
