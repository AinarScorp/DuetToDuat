using UnityEngine;

//Made by Amos Johan Persson

namespace MainGame.SpatialUI
{
    public class UrnPopupEventListener : MonoBehaviour
    {
        [SerializeField] PopUpController popUp;

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

            Managers.GameManager gameManager = GameObject.FindWithTag("GameManager")?.GetComponent<Managers.GameManager>();
            if (gameManager == null)
                return;

            gameManager.OnStartCinematics += popUp.Disable;
            gameManager.OnStartCinematics += () => popUp.SetAllAlphas(0);
            gameManager.OnFinishCinematics += popUp.Enable;
        }
    }
}
