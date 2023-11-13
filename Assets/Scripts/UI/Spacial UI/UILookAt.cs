using UnityEngine;

//Made by Amos Johan Persson

namespace MainGame.SpatialUI
{
    public class UILookAt : MonoBehaviour
    {
        [SerializeField] GameObject backgroundObject;

        [Tooltip("Controls how far away from the object the sign will be.")]
        [SerializeField] float offset;


        PopUpController popUp;
        GameObject followTarget;


        private void Start()
        {
            popUp = GetComponent<PopUpController>();
            followTarget = popUp.GetPlayer();

            if (backgroundObject == null)
                return;

            Vector3 originalPosition = backgroundObject.transform.position;
            backgroundObject.transform.position = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z + offset);
        }

        void Update()
        {
            TurnTowardsTarget(followTarget);
        }

        private void TurnTowardsTarget(GameObject target)
        {
            Vector3 lookAtTarget = target.transform.position;
            Vector3 yLockedTarget = new Vector3(lookAtTarget.x, transform.position.y, lookAtTarget.z);
            transform.LookAt(yLockedTarget);
        }
    }

}