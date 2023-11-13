using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MainGame.Audio;
using MainGame.Inputs;
using MainGame.Movement;
using UnityEngine;
using AudioType = MainGame.Audio.AudioType;

//Made by Einar Hallik
namespace MainGame.Spirit
{
    public class Possesser : MonoBehaviour
    {
        [SerializeField][Range(0,10)] float sphereCastRadius = 2;
        [SerializeField][Range(0,100)] float maxPossessDistance;
        [SerializeField] CinemachineVirtualCamera[] cinemachines;
        [SerializeField] Transform meshHolder;
        [SerializeField] LayerMask possessibleLayers;
        PlayerController playerController;
        PhysicsController physicsController;
        InputHandler inputHandler;
        IPossessable possessable;
        AudioSource audioSource;
        
        Transform spiritDefaultCameraTarget;
        public event Action OnPossessionStarted;
        public event Action OnPossessionFinished;
        public event Action OnEjectionStarted;
        public event Action OnEjectionFinished;

        void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            physicsController = GetComponent<PhysicsController>();
            playerController = GetComponent<PlayerController>();
            audioSource = GetComponentInChildren<AudioSource>();
        }

        void Start()
        {
            spiritDefaultCameraTarget = playerController.ThirdPersonCamTarget;
            inputHandler.StoppedShooting += PossessObject;
            OnPossessionStarted += () =>
            {
                if (audioSource == null) return;
                AudioManager.Instance?.PlayAudio(AudioType.Possession, audioSource);
            };
        }

        
        void Update()
        {
            if (!inputHandler.ShootInput)
            {
                return;
            }

            if (!Physics.SphereCast(spiritDefaultCameraTarget.position, sphereCastRadius, spiritDefaultCameraTarget.forward, out RaycastHit hit, maxPossessDistance))
            {
                return;
            }

            if (!hit.collider.gameObject.TryGetComponent(out IPossessable possessableVisuals))
            {
                return;
            }
            
            possessableVisuals.VisualisePossession();
            
        }

        void PossessObject()
        {
            if (!Physics.SphereCast(spiritDefaultCameraTarget.position, sphereCastRadius, spiritDefaultCameraTarget.forward, out RaycastHit hit, maxPossessDistance,possessibleLayers))
            {
                return;
            }

            if (!hit.collider.gameObject.TryGetComponent(out possessable))
            {
                return;
            }

            if (possessable == null)
            {
                return;
            }
            
            OnPossessionStarted?.Invoke();
            possessable.Possess();
            ToggleSpirit(false);
            this.transform.position = possessable.GetShoulder().position;
            SetCamera(possessable.GetShoulder());
            SwitchBetweenActions(true);
            OnPossessionFinished?.Invoke();
        }

        void EjectFromPossession()
        {
            OnEjectionStarted?.Invoke();
            possessable.UnPossess();
            possessable = null;
            SetCamera(spiritDefaultCameraTarget);
            SwitchBetweenActions(false);
            OnEjectionFinished?.Invoke();
        }

        void ToggleSpirit(bool turnOn)
        {
            meshHolder.gameObject.SetActive(turnOn);
            physicsController.SetEnabled(turnOn);
        }

        void SetCamera(Transform shoulder)
        {
            foreach (var cinemachine in cinemachines)
            {
                cinemachine.Follow = shoulder;
            }
            
            playerController.SetThirdPersonCamTarget(shoulder);
        }

        void SwitchBetweenActions(bool isPossessing)
        {
            if (isPossessing)
            {
                inputHandler.StoppedShooting -= PossessObject;
                inputHandler.StoppedShooting += EjectFromPossession;

                inputHandler.StoppedAiming += EjectFromPossession;
                

            }
            else
            {

                inputHandler.StoppedAiming -= EjectFromPossession;
                inputHandler.StoppedShooting -= EjectFromPossession;
                inputHandler.StoppedShooting += PossessObject;
                
            }
        }
        
 
    }
}