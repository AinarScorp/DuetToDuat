using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Inputs;
using MainGame.Movement;
using UnityEngine;

//Made by Einar Hallik
namespace MainGame.WorldInterraction
{
    public class ItemPusher : MonoBehaviour
    {
        [SerializeField] float sphereGrabCheckRadius = 1.5f;
        [SerializeField] float grabDistanceMax = 2;
        [SerializeField] float rotationSmoothTimeWhilePushing = 0.7f;
        float defaultRotationSmoothTime;
        PlayerController playerController;
        InputHandler inputHandler;
        float playerCenterYOffset;

        public event Action OnGrabbedItem;
        public event Action OnReleasedItem;
        UrnItem pushableItem;
        Vector3 playerCenter => new(transform.position.x, transform.position.y + playerCenterYOffset, transform.position.z);

        void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            playerController = GetComponent<PlayerController>();
        }
        

        void Start()
        {
            defaultRotationSmoothTime = playerController.RotationSmoothTime;
            playerCenterYOffset = GetComponent<CharacterController>().height / 2;
            inputHandler.StartedAiming += GrabPushableItem;
            inputHandler.StoppedAiming += ReleasePushableItem;
        }

        void GrabPushableItem()
        {
            #region Guards

            if (!Physics.SphereCast(playerCenter, sphereGrabCheckRadius, this.transform.forward, out RaycastHit hit, grabDistanceMax))
            {
                return;
            }
            if (!hit.collider.gameObject.TryGetComponent(out pushableItem))
            {
                return;
            }
            if (!pushableItem.IsMovable)
            {
                return;
            }
            #endregion

            playerController.SetRotationSmoothTime(rotationSmoothTimeWhilePushing);
            pushableItem.pusherRef = this.transform;
            pushableItem.Grab();
            OnGrabbedItem?.Invoke();
        }

        void ReleasePushableItem()
        {
            if (pushableItem == null)
            {
                return;
            }

            playerController.SetRotationSmoothTime(defaultRotationSmoothTime);

            pushableItem.Release();
            pushableItem = null;
            OnReleasedItem?.Invoke();

        }
    }
}

#region COmmented out

//Wrong pushing
// [SerializeField] float pushForce = 1;
//
// Vector3 position => this.transform.position;
// void OnControllerColliderHit(ControllerColliderHit hit)
// {
//     Rigidbody rb = hit.collider.attachedRigidbody;
//     if (!rb )
//     {
//         return;
//     }
//     PushableItem pushableItem = rb.GetComponent<PushableItem>();
//     if (!pushableItem || !pushableItem.IsMovable)
//     {
//         return;
//     }
//     
//     Vector3 forceDirection = hit.gameObject.transform.position - position;
//     forceDirection.y = 0;
//     forceDirection.Normalize();
//     rb.AddForceAtPosition(forceDirection * pushForce, position, ForceMode.Impulse);
//     
// }

#endregion