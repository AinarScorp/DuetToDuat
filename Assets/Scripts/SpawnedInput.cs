using System;
using System.Collections.Generic;
using Cinemachine;
using MainGame.Audio;
using MainGame.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using MainGame.Movement;
using UnityEngine.SceneManagement;
using AudioType = MainGame.Audio.AudioType;

//Made by Einar Hallik
namespace MainGame.Inputs
{
    public class SpawnedInput : MonoBehaviour
    {
        [SerializeField] Camera spawnedCamera;
        
        //cached
        PlayerInput playerInput;
        
        //variables
        PlayerType playerType = PlayerType.NotInitialised;
        int cinemachineLayer = -99;
        
        public enum PlayerType
        {
            NotInitialised,Spirit, Body
        }

        #region Setup

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += (arg0, mode) =>
            {
                SetupSpawnedInput(playerInput, playerType, cinemachineLayer);
                print("Scene loaded");
            };

        }


        public void OnDeviceLostReconnect()
        {
            ConnectInputs();
        }
        public void SetupSpawnedInput(PlayerInput newPlayerInput, PlayerType newPlayerType, int newCinemachineLayer)
        {
            playerType = newPlayerType;
            playerInput = newPlayerInput;
            cinemachineLayer = newCinemachineLayer;
            ConnectInputs();
            GameManager gameManager = GameObject.FindWithTag("GameManager")?.GetComponent<GameManager>();
            if (gameManager == null)
            {
                spawnedCamera.enabled = false;
                return;
            }

            gameManager.OnStartCinematics += () =>spawnedCamera.enabled = false;
            gameManager.OnFinishCinematics += () =>
            {
                if (spawnedCamera != null)
                {
                    spawnedCamera.enabled = true;
                }
            };
        }
        
        void ConnectInputs()
        {
            #region Guards
            if (playerInput ==null)
            {
                Debug.LogError("Player input is NUll");
                return;
            }
            if (playerType == PlayerType.NotInitialised)
            {
                Debug.LogError("playerType is Not Initialised");
                return;
            }

            if (cinemachineLayer == -99)
            {
                Debug.LogError("cinemachine Layer is Not set");
                return;
            }
            
            string tagToConnectInputs = playerType == PlayerType.Body ? "BodyPlayer" : "SpiritPlayer";
            string tagToConnectCinemachine = playerType == PlayerType.Body ? "CM Body" : "CM Spirit";

            CinemachineVirtualCamera[] cinemachines = GetCorrectCinemachineByTag(tagToConnectCinemachine);
            if (cinemachines.Length<= 0)
            {
                Debug.LogWarning("If this is not a level to play, igonre this message, if not then your Cinemachine in the level is not set");
                return;
            }
            InputHandler inputHandler = GetCorrectInputHandlerByTag(tagToConnectInputs);
            if (inputHandler == null)
            {
                Debug.LogWarning("If this is not a level to play, igonre this message, if not then your Players in the level are not set");
                return;
            }
            #endregion
            
            //Assign correct Layer to the cinemachine gameobject
            foreach (var cinemachine in cinemachines)
            {
                cinemachine.gameObject.layer = cinemachineLayer;
            }
            // //Connect inputs to the correct controller
            inputHandler.SetupInputs(playerInput);
            AttachCameraComponentToPlayerController(inputHandler);
            
            
            //connect cinemachineBrain to correct Cinemachine
            spawnedCamera.cullingMask |= 1 << cinemachineLayer;
            if (playerType == PlayerType.Spirit)
            {
                spawnedCamera.cullingMask |= (1 << LayerMask.NameToLayer("Spitit Vision")) | (1 << LayerMask.NameToLayer("SpiritVisibleInterractible"));
            }
        }

        #endregion


        void AttachCameraComponentToPlayerController(InputHandler inputHandler)
        {
            inputHandler.GetComponent<PlayerController>().cameraMain = spawnedCamera;
        }

        InputHandler GetCorrectInputHandlerByTag(string inputHandlerTag)
        {
            return GameObject.FindWithTag(inputHandlerTag).GetComponent<InputHandler>();
        }

        CinemachineVirtualCamera[] GetCorrectCinemachineByTag(string cinemachineTag)
        {
            GameObject[] objectsWithTags = GameObject.FindGameObjectsWithTag(cinemachineTag);
            List<CinemachineVirtualCamera> cinemachines = new List<CinemachineVirtualCamera>();
            foreach (var potentaialCinemachine in objectsWithTags)
            {
                if (potentaialCinemachine.TryGetComponent(out CinemachineVirtualCamera cinemachineToAdd))
                {
                    cinemachines.Add(cinemachineToAdd);
                }
            }
            return cinemachines.ToArray();

        }
        

    }
    
}
