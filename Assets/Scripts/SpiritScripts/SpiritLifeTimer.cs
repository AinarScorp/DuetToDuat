using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Movement;
using UnityEngine;

// Made by Max Ekberg.

// TODO Delete this.
public class SpiritLifeTimer : TimerBase
{
    [SerializeField] private Transform physicalBodyPlayer;

    private CharacterController characterController;
    private PhysicsController spiritController;
    private MeshRenderer spiritRenderer;
    private CapsuleCollider spiritCollider;

    private void OnEnable()
    {
        characterController = GetComponent<CharacterController>();
        spiritController = GetComponent<PhysicsController>();
        spiritRenderer = GetComponentInChildren<MeshRenderer>();
        spiritCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if(timerDone) OnTimerDone();
    }

    protected override void OnTimerDone()
    {
        base.OnTimerDone();
        characterController.enabled = false;
        spiritController.enabled = false;
        spiritRenderer.gameObject.SetActive(false);
        spiritCollider.enabled = false;

        transform.position = physicalBodyPlayer.position;
    }
}
