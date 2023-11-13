using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Audio;
using MainGame.Spirit;
using UnityEngine;
using UnityEngine.Serialization;
using AudioType = MainGame.Audio.AudioType;

//Made by Einar Hallik

namespace MainGame.WorldInterraction
{
    [RequireComponent(typeof(PossessibleItem))]
    [RequireComponent(typeof(Rigidbody))]
    public class UrnItem : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 6;
        [SerializeField] float distance = 5;

        Rigidbody rb;
        AudioSource audioSource;

        [HideInInspector] public Transform pusherRef;
        bool grabbed;
        bool isMovable;

        public bool IsMovable => isMovable;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponentInChildren<AudioSource>();
        }


        void Start()
        {
            PossessibleItem possessibleItem = GetComponent<PossessibleItem>();
            possessibleItem.OnPossessed += () => { isMovable = true; };
            possessibleItem.OnUnPossessed += () => { isMovable = false; };
            
        }


        void Update()
        {
            if (!grabbed)
            {
                return;
            }

            Vector3 posToReach = pusherRef.position + pusherRef.forward * distance;
            Vector3 newPos = Vector3.Lerp(transform.position, posToReach, Time.deltaTime * moveSpeed);
            transform.position = newPos;
        }




        public void Grab()
        {
            AudioManager.Instance?.PlayAudio(AudioType.UrnMoving, audioSource);
            grabbed = true;
            rb.useGravity = false;
        }

        public void Release()
        {
            AudioManager.Instance?.StopAudio(AudioType.UrnMoving, audioSource);
            grabbed = false;
            rb.useGravity = true;
        }



    }
}