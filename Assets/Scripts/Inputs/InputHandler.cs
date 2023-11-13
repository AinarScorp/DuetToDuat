using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Made by Einar Hallik
namespace MainGame.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        #region Variables
        
        //bool jumpInput;
        bool isAiming;
        bool shootInput;
        Vector2 movementInputs;
        Vector2 lookInput;
        
        
        //cached
        InputActionAsset inputActionAsset;
        InputActionMap inputActionMap;
        #endregion

        public event Action StoppedAiming;
        public event Action StartedAiming;
        public event Action StoppedShooting;
        public event Action StartedShooting;
        public event Action InputsDisabled;
        
        #region Properties
        public bool IsAiming => isAiming;
        public bool ShootInput => shootInput;
        public Vector2 MovementInputs => movementInputs;
        public Vector2 LookInput => lookInput;

        #endregion


        #region Setup

        public void SetupInputs(PlayerInput playerInput)
        {
            inputActionAsset = playerInput.actions;
            inputActionMap = inputActionAsset.FindActionMap("Main");
            
            inputActionMap.FindAction("Move").performed += ctx => movementInputs = ctx.ReadValue<Vector2>();
            inputActionMap.FindAction("Move").canceled += ctx => movementInputs *= 0;
            inputActionMap.FindAction("Look").performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            
            inputActionMap.FindAction("Shoot").performed += ctx =>
            {
                StartedShooting?.Invoke();
                shootInput = ctx.ReadValue<float>() > 0.1f;
            };
            inputActionMap.FindAction("Shoot").canceled += ctx =>
            {

                shootInput = false;
                StoppedShooting?.Invoke();
            };
            
            inputActionMap.FindAction("Aim").performed += ctx =>
            {
                StartedAiming?.Invoke();
                isAiming = ctx.ReadValue<float>() > 0.1f;
            };
            inputActionMap.FindAction("Aim").canceled += ctx =>
            {
                isAiming = false;
                StoppedAiming?.Invoke();
            };
            
            this.enabled = true;
            SubscribeResetBooleans();

        }

        void Start()
        {
            SubscribeToPauseEvents();
        }

        void SubscribeToPauseEvents()
        {
            Managers.GameManager gameManager = GameObject.FindWithTag("GameManager")?.GetComponent<Managers.GameManager>();
            if (gameManager == null)
            {
                return;
            }
            gameManager.OnGamePaused += () => this.enabled = false;
            gameManager.OnGameUnPaused += () => this.enabled = true; 

            gameManager.OnStartCinematics += () => this.enabled = false;
            gameManager.OnFinishCinematics += () => this.enabled = true;
        }

        void OnEnable()
        {
            if (!InputsAreConnected())
            {
                this.enabled = false;
                return;
            }
            inputActionMap?.Enable();
        }

        void OnDisable()
        {
            if (!InputsAreConnected())
            {
                this.enabled = false;
                return;
            }
            inputActionMap?.Disable();
            InputsDisabled?.Invoke();
        }

        #endregion
        

        void SubscribeResetBooleans()
        {
            InputsDisabled += () =>
            {
                lookInput *= 0;
                movementInputs *= 0;
                isAiming = false;
                shootInput = false;
            };
        }

        bool InputsAreConnected()
        {
            return (inputActionMap != null && inputActionAsset != null);
        }
    }
}