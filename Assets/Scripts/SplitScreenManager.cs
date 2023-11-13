using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Made by Einar Hallik
namespace MainGame.Inputs
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class SplitScreenManager : MonoBehaviour
    {
        [SerializeField] List<LayerMask> playerLayers;
        //cached
        PlayerInputManager playerInputManager;

        void OnEnable()
        {
            GetPlayerInputManagerComponent();
            playerInputManager.onPlayerJoined += AddPlayer;
        }



        void OnDisable()
        {
            GetPlayerInputManagerComponent();
            playerInputManager.onPlayerJoined -= AddPlayer;
        }

        public void SetEnable(bool turnOn) => this.gameObject.SetActive(turnOn);

        void AddPlayer(PlayerInput playerInput)
        {
            int playerIndex = playerInput.playerIndex;
            int layerToAdd = (int)Mathf.Log(playerLayers[playerIndex].value, 2);
            playerInput.GetComponent<SpawnedInput>().SetupSpawnedInput(playerInput,GetPlayerType(playerIndex),layerToAdd);
        }

        SpawnedInput.PlayerType GetPlayerType(int index)
        {
            return index == 0 ? SpawnedInput.PlayerType.Body : SpawnedInput.PlayerType.Spirit;

        }
        
        void GetPlayerInputManagerComponent()
        {
            if (playerInputManager == null)
            {
                playerInputManager = GetComponent<PlayerInputManager>();
            }
        }
        
    }
}