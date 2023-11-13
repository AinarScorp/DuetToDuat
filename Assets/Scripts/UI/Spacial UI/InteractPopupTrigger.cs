
using UnityEngine;

//Made by Amos Johan Persson

namespace MainGame.SpatialUI
{
    public class InteractPopupTrigger : MonoBehaviour
    {
        [SerializeField] PopUpController popUp;

        [Tooltip("Is the trigger for the spirit (true) or for the body (false)? Not updated live.")]
        [SerializeField] bool forSpirit;

        string playerTag;

        private void Start()
        {
            if (forSpirit)
            {
                playerTag = "SpiritPlayer";
            }
            else
            {
                playerTag = "BodyPlayer";
            }
        }


        private bool ActivationCheck(Collider activator)
        {
            return activator.CompareTag(playerTag);
        }

        private void OnTriggerEnter(Collider other)
        {
            if( ActivationCheck(other) && popUp.IsReactivationEnabled())
            {
                popUp.StartFadeIn();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (ActivationCheck(other) && popUp.IsReactivationEnabled())
            {
                popUp.StartFadeAway();
            }
        }
    }

}
