using System;
using UnityEngine;
using MainGame.Death;
//Made by Einar Hallik

namespace MainGame.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(CustomGravity))]
    public class PhysicsController : MonoBehaviour
    {
        CharacterController characterController;
        PlayerController playerController;
        CustomGravity customGravity;
        
        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerController = GetComponent<PlayerController>();
            customGravity = GetComponent<CustomGravity>();
        }

        private void Start()
        {
            DeathScript deathScript = GetComponent<DeathScript>();
            deathScript.OnPlayerDied += () => SetEnabled(false);
            deathScript.OnPlayerRevive += () => SetEnabled(true);
        }

        void Update()
        {
            SupplyCharacterController();
        }

        public void SetEnabled(bool turnOn)
        {
            characterController.enabled = turnOn;
            this.enabled = turnOn;
        }
        void SupplyCharacterController()
        {
            characterController.Move(GetInputMotion() + GetGravityMotion());
        }
        
        Vector3 GetGravityMotion()
        {
            return customGravity.GetVerticalVelocity() * Time.deltaTime;
        }
        Vector3 GetInputMotion()
        {
            playerController.GetDirectionAndSpeed(out Vector3 moveDirection, out float horizontalSpeed);
            //if (!playerController.enabled) return Vector3.zero;
            
  
            return moveDirection * (horizontalSpeed * Time.deltaTime);
        }
    }
    
}
