using System;
using System.Collections;
using Cinemachine;
using MainGame.Audio;
using MainGame.Inputs;
using MainGame.Movement;
using MainGame.Death;
using UnityEngine;
using AudioType = MainGame.Audio.AudioType;

//Made by Einar Hallik
//TODO clean coroutine
namespace MainGame.Spirit
{
    [RequireComponent(typeof(PhysicsController))]
    [RequireComponent(typeof(InputHandler))]
    public class SpiritTeleport : MonoBehaviour
    {
        [Header("Tuning")] [SerializeField] float teleportationSpeed = 2f;
        [SerializeField] float teleportMaxDistance = 100.0f;
        [SerializeField] LayerMask obsttuctionLayers;

        [Header("Use this not to be stuck in the wall")]
        [SerializeField]
        float playerRadius = 1f;

        [Header("Settings")] [SerializeField] Transform teleportIndicator;
        [SerializeField] CinemachineVirtualCamera aimCinemachine;

        [Header("Stuff To Turn On Upon Teleportation")]
        [SerializeField]
        GameObject[] thingsToEnable;

        [Header("Stuff To Turn Off Upon Teleportation")]
        [SerializeField]
        GameObject[] thingsToDisable;

        [SerializeField] float ejectionDistance = 10;

        PlayerController playerController;
        InputHandler inputHandler;
        PhysicsController physicsController;
        DeathScript deathScript;
        AudioSource audioSource;
        CustomGravity customGravity;
        Coroutine TeleportationCoroutine;

        Vector3 teleportationPoint;
        float playerCenterYOffset;
        bool isPossessing;

        Transform shoulder => playerController.ThirdPersonCamTarget;
        Vector3 playerCenter => new(0, playerCenterYOffset, 0);

        void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            physicsController = GetComponent<PhysicsController>();
            playerController = GetComponent<PlayerController>();
            deathScript = GetComponent<DeathScript>();
            audioSource = GetComponentInChildren<AudioSource>();
            customGravity = GetComponent<CustomGravity>();
        }

        void Start()
        {
            teleportIndicator.gameObject.SetActive(false);
            playerCenterYOffset = GetComponent<CharacterController>().height / 2;

            SubscribeToAimInputs();
            SubscribeToPossessionEvents();
            SubscribeToDeathEvents();
        }

        void SubscribeToAimInputs()
        {
            inputHandler.StoppedAiming += Teleport;
            inputHandler.StartedAiming += MoveIndicator;
        }

        void SubscribeToPossessionEvents()
        {
            Possesser possesser = GetComponent<Possesser>();
            possesser.OnPossessionStarted += SubscribeToPossess;
            possesser.OnPossessionFinished += () => { isPossessing = true; };

            possesser.OnEjectionStarted += SubscribeOnEjection;
            possesser.OnEjectionFinished += () => { isPossessing = false; };
        }

        void SubscribeToDeathEvents()
        {
            deathScript.OnPlayerDied += () =>
            {
                inputHandler.StoppedAiming -= Teleport;
                inputHandler.StartedAiming -= MoveIndicator;
            };

            deathScript.OnPlayerRevive += () =>
            {
                inputHandler.StoppedAiming += Teleport;
                inputHandler.StartedAiming += MoveIndicator;
            };
        }


        void SubscribeOnEjection()
        {
            inputHandler.StoppedShooting -= Teleport;
            inputHandler.StartedShooting -= MoveIndicator;
        }

        void SubscribeToPossess()
        {
            inputHandler.StoppedShooting += Teleport;
            inputHandler.StartedShooting += MoveIndicator;
        }


        void MoveIndicator()
        {
            if (!isPossessing && !customGravity.IsGrounded())
            {
                return;
            }
            teleportIndicator.gameObject.SetActive(true);
            aimCinemachine.enabled = true;
        }


        void Teleport()
        {
            if (!isPossessing && !customGravity.IsGrounded())
            {
                return;
            }

            aimCinemachine.enabled = false;
            teleportIndicator.gameObject.SetActive(false);
            if (TeleportationCoroutine != null)
            {
                StopCoroutine(TeleportationCoroutine);
            }

            TeleportationCoroutine = StartCoroutine(PerformTeleportation());
        }

        void Update()
        {
            if (!isPossessing && !inputHandler.IsAiming)
            {
                return;
            }

            if (!inputHandler.IsAiming && !inputHandler.ShootInput)
            {
                return;
            }
            
            AimToTeleportationPoint();
        }

        void AimToTeleportationPoint()
        {
            Vector2 cameraCenter = new Vector2(Screen.width * 0.75f, Screen.height * 0.5f);
            Ray ray = playerController.cameraMain.ScreenPointToRay(cameraCenter);
            float distanceToUse = isPossessing ? ejectionDistance : teleportMaxDistance;
            
            bool raycastSucceeded = Physics.Raycast(ray, out RaycastHit hit, distanceToUse, obsttuctionLayers);
            
            teleportationPoint = raycastSucceeded? hit.point : ray.origin + ray.direction * distanceToUse;
            
            if (teleportIndicator != null)
            {
                teleportIndicator.position = teleportationPoint;
            }
        }

        IEnumerator PerformTeleportation()
        {
            AudioManager.Instance?.PlayAudio(AudioType.SpiritTeleport, audioSource);
            physicsController.SetEnabled(false);
            
            if (isPossessing)
            {
                transform.position = shoulder.position + playerCenter;
            }

            ToggleGameObjects(true);
            yield return LerpThroughPosition();
            TeleportationCoroutine = null;
            physicsController.SetEnabled(true);
            customGravity.ResetGravity();
            ToggleGameObjects(false);
        }

        void ToggleGameObjects(bool startedTeleporting)
        {
            foreach (var gameObjectToDisable in thingsToDisable)
            {
                gameObjectToDisable.SetActive(!startedTeleporting);
            }

            foreach (var gameObjectToEnable in thingsToEnable)
            {
                gameObjectToEnable.SetActive(startedTeleporting);
            }
        }

        IEnumerator LerpThroughPosition()
        {
            Vector3 startPos = !isPossessing ? transform.position : shoulder.position + playerCenter;

            Vector3 endPos = teleportationPoint;

            float pathMagnitude = (endPos - startPos).magnitude;
            Vector3 directionToEnd = (endPos - startPos).normalized;
            endPos = startPos + directionToEnd * (pathMagnitude - playerRadius);

            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * teleportationSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, percent);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}