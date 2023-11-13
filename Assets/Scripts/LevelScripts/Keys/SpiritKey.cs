using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using UnityEngine;
using UnityEngine.VFX;

// Made by Max Ekberg.
namespace MainGame.Keys
{
    public class SpiritKey : Key
    {
        int amountOfActivators = 0;
        
        // Override bool to make sure only SpiritPlayer can activate key.
        protected override bool ActivationCheck(Collider activator)
        {
            return activator.CompareTag("SpiritPlayer");
        }

        // Deactivates key when collider leaves if activation isn't permanent.
        private void OnTriggerEnter(Collider other)
        {
            if (!ActivationCheck(other)) return;
            
            amountOfActivators++;
            
            IsActivated = amountOfActivators > 0;

            if (IsActivated && amountOfActivators == 1)
            {
                Activate();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isActivationPermanent || amountOfActivators == 0 || !ActivationCheck(other)) return;

            amountOfActivators--;
            
            IsActivated = amountOfActivators > 0;

            if (!IsActivated)
            {
                Deactivate();
            }
        }
    }
}
