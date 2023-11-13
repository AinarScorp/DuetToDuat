using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using MainGame.Movement;
using MainGame.Spirit;
using UnityEngine;
using AudioType = MainGame.Audio.AudioType;
//Made by Einar Hallik

namespace MainGame.Vessel
{
    public class VesselPlayer : MonoBehaviour, IPossessable
    {
        Transform shoulder;

        void Awake()
        {
            PlayerController playerController = GetComponent<PlayerController>();
            shoulder = Instantiate(playerController.ThirdPersonCamTarget, playerController.ThirdPersonCamTarget.position, Quaternion.identity, this.transform);
        }

        
        void Start()
        {
            
            AudioManager.Instance?.PlayAudio(AudioType.BodyMoan, GetComponentInChildren<AudioSource>());
        }

        public Transform GetShoulder()
        {
            return shoulder;
        }

        public void VisualisePossession()
        {
            
        }

        public void Possess()
        {
        }

        public void UnPossess()
        {
        }
    }
}
