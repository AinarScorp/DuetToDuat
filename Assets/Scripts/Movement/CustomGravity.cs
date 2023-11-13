using System;
using UnityEngine;
using MainGame.Inputs;
using UnityEngine.Serialization;

//Made by Einar Hallik

namespace MainGame
{
    public class CustomGravity : MonoBehaviour
    {
        [SerializeField] float gravity = -15.0f;
        [SerializeField] float heavierGravity = -30.0f;
        [SerializeField] float gravityMultiplier = 2;
        [SerializeField] float groundedRadius = 0.5f;
        [SerializeField] LayerMask groundLayers;
        
        //cached
        InputHandler inputHandler;
        
        float verticalVelocity;

        
        void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
        }

        void OnEnable()
        {
            verticalVelocity = 0;
        }

        void Update()
        {
            ChangeVelocityByGravity();
        }

        public Vector3 GetVerticalVelocity()
        {
            return new Vector3(0.0f, verticalVelocity, 0.0f);
        }

        void ChangeVelocityByGravity()
        {
            float pushDownMultiplier = verticalVelocity < 0.0f ? gravityMultiplier : 1;
            float gravityToUse = gravity;
            if (verticalVelocity < 0.0f)
            {
                if (IsGrounded())
                {
                    verticalVelocity = -2.0f;
                    pushDownMultiplier = 1f;
                }

                gravityToUse *= pushDownMultiplier;
            }
            
            //else if (!IsGrounded() && inputHandler.enabled && !inputHandler.JumpInput)
            else if (!IsGrounded() && inputHandler.enabled)
            {
                gravityToUse = heavierGravity;
            }


            verticalVelocity += gravityToUse * Time.deltaTime;
        }

        public void ApplyJumpVelocity(float heightToReach)
        {
            verticalVelocity = Mathf.Sqrt(gravity * heightToReach * -2f);

        }
        public bool IsGrounded()
        {
            return Physics.CheckSphere(transform.position, groundedRadius, groundLayers);
        }

        public void ResetGravity()
        {
            verticalVelocity = 0;
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, groundedRadius);
            
        }
    }
    
}
