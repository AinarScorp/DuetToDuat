using System.Collections;
using System.Collections.Generic;
using MainGame.Keys;
using UnityEngine;

// Made by Max Ekberg.
namespace MainGame.Keys
{
    public class HeartKey : Key
    {
        protected override bool ActivationCheck(Collider activator)
        {
            return activator.CompareTag("HeartWinTrigger");
        }
        
        // Activates the key if the collider isn't the spirit player.
        void OnTriggerEnter(Collider other)
        {
            IsActivated = ActivationCheck(other);

            if (IsActivated)
            {
                Activate();
            }
            
        }
    }
}
