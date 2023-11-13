using System;
using UnityEngine;
using MainGame.Inputs;
using UnityEngine.Serialization;

//Made by Einar Hallik
// •••ToDo Your mainCamera is public, you need to change that
namespace MainGame.Movement
{
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(CustomGravity))]
    public class PlayerController : MonoBehaviour
    {
        #region Exposed variables

        [Header("PlayerSettings")] //
        [SerializeField] float moveSpeed = 2f;


        [Header("Rotation")] //
        [SerializeField]
        float rotationSmoothTime = 0.1f;
        
        
        [Header("Camera Settings")] //
        [SerializeField]
        Transform thirdPersonCamTarget;

        

        [SerializeField] float cameraTopClamp = 50f, cameraBottomClamp = -50f;
        [SerializeField] float cameraRotationSpeed = 0.5f;


        [Header("Jumping")] //
        [SerializeField] float windMultilpier = 0.5f;
        // [SerializeField] float heightToReach = 5f;
        //
        // [SerializeField] float jumpCooldown = 0.5f;


        [Header("Animations")]
        [SerializeField]
        Animator animator;

        #endregion

        #region variables

        //Rotations
        float targetAngle;
        bool rotateOnMove = false;

        //Jumping
        float jumpCooldownTimer;

        // Camera Cinemachine
        public Camera cameraMain; //CHANGE LATER
        float cameraYaw, cameraPitch;
        float turnSmoothVelocity;

        //cached
        CustomGravity customGravity;
        InputHandler inputHandler;

        #endregion

        #region Properties
        
        public Transform ThirdPersonCamTarget => thirdPersonCamTarget;
        //public Transform FirstPersonCamTarget => _firstPersonCamTarget;
        float mainCameraYaw => cameraMain.transform.eulerAngles.y;

        public float RotationSmoothTime => rotationSmoothTime;

        #endregion


        #region Unity Logic

        void Awake()
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            inputHandler = GetComponent<InputHandler>();
            customGravity = GetComponent<CustomGravity>();

            inputHandler.StartedAiming += () => rotateOnMove = true;
            inputHandler.StoppedAiming += () => rotateOnMove = false;
            inputHandler.StartedShooting += () => rotateOnMove = true;
            inputHandler.StoppedShooting += () => rotateOnMove = false;
            
            
        }


        void Start()
        {
            cameraYaw = thirdPersonCamTarget.rotation.eulerAngles.y;
        }


        void Update()
        {
            HandleRotation();
            //Jump();

        }

        void LateUpdate()
        {
            HandleCameraRotation();
        }

        #endregion

        #region Handlers

        public void GetDirectionAndSpeed(out Vector3 moveDirection, out float horizontalSpeed)
        {
            // if (inputHandler.ShootInput)
            // {
            //     horizontalSpeed = 0;
            //     moveDirection = Vector3.zero;
            //     return;
            // }

            //horizontalSpeed = inputHandler.MovementInputs == Vector2.zero || inputHandler.IsAiming ? 0.0f : moveSpeed;
            horizontalSpeed = inputHandler.MovementInputs == Vector2.zero ? 0.0f : moveSpeed;
            if (!customGravity.IsGrounded())
            {
                horizontalSpeed *= windMultilpier;
            }

            Vector3 direction = rotateOnMove ? new Vector3(inputHandler.MovementInputs.x, 0, inputHandler.MovementInputs.y).normalized : Vector3.forward;
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * direction;
            moveDirection.Normalize();

            if (animator != null)
            {
                
                animator.SetBool("isMoving", horizontalSpeed > 0.1f);
                animator.SetFloat("movingForward",direction.z);
                animator.SetFloat("movingRight",direction.x);
                
            }
        }

        void HandleRotation()
        {
            if (inputHandler.MovementInputs != Vector2.zero || rotateOnMove)
            {
                float moveInputX = rotateOnMove ? 0.0f : inputHandler.MovementInputs.x;
                float moveInputY = rotateOnMove ? 0.0f : inputHandler.MovementInputs.y;
                Vector3 moveInputs = new Vector3(moveInputX, 0.0f, moveInputY).normalized;

                targetAngle = Mathf.Atan2(moveInputs.x, moveInputs.z) * Mathf.Rad2Deg + mainCameraYaw;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }


        void HandleCameraRotation()
        {
            if (inputHandler.LookInput.sqrMagnitude >= 0.01f)
            {
                cameraYaw += inputHandler.LookInput.x * cameraRotationSpeed;
                cameraPitch += inputHandler.LookInput.y * cameraRotationSpeed;
            }

            cameraYaw = ClampAngle(cameraYaw, float.MinValue, float.MaxValue);
            cameraPitch = ClampAngle(cameraPitch, cameraBottomClamp, cameraTopClamp);

            // if (_inputHandler.IsAiming)
            //     _firstPersonCamTarget.rotation = Quaternion.Euler(-_cameraPitch, _cameraYaw, 0.0f);
            // else
            thirdPersonCamTarget.rotation = Quaternion.Euler(-cameraPitch, cameraYaw, 0.0f);
        }


        // void Jump()
        // {
        //     if (jumpCooldownTimer >= 0.0f) jumpCooldownTimer -= Time.deltaTime;
        //     if (inputHandler.ShootInput) return;
        //     if (!inputHandler.JumpInput) return;
        //     if (inputHandler.IsAiming) return;
        //     if (!customGravity.IsGrounded() || jumpCooldownTimer >= 0.0f) return;
        //     customGravity.ApplyJumpVelocity(heightToReach);
        //     jumpCooldownTimer = jumpCooldown;
        // }

        #endregion


        float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public void SetRotationSmoothTime(float newValue)
        {
            rotationSmoothTime = newValue;
        }

        public void SetThirdPersonCamTarget(Transform newTarget)
        {
            thirdPersonCamTarget = newTarget;
        }
    }
}