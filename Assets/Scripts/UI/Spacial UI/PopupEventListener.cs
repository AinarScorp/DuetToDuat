using UnityEngine;

//Made by Amos Johan Persson

namespace MainGame.SpatialUI
{
    public class PopupEventListener : MonoBehaviour
    {
        [SerializeField] PopUpController popUp;
        [SerializeField] Keys.Key interactableKey;
        [SerializeField] bool resettable;

        void Start()
        {
            if (popUp == null)
                return;

            var possesserScript = popUp.GetPlayer().GetComponent<Spirit.Possesser>();

            if (possesserScript != null)
            {
                possesserScript.OnPossessionStarted += popUp.Disable;
                possesserScript.OnPossessionStarted += () => popUp.SetAllAlphas(0);
                possesserScript.OnEjectionFinished += popUp.Enable;
            }

            if (interactableKey == null)
                return;

            interactableKey.OnActivated += popUp.Disable;

            if (resettable)
                interactableKey.OnDeactivated += popUp.Enable;

            Managers.GameManager gameManager = GameObject.FindWithTag("GameManager")?.GetComponent<Managers.GameManager>();
            if (gameManager == null)
                return;

            gameManager.OnStartCinematics += popUp.Disable;
            gameManager.OnStartCinematics += () => popUp.SetAllAlphas(0);
            gameManager.OnFinishCinematics += popUp.Enable;
        }
    }
}
