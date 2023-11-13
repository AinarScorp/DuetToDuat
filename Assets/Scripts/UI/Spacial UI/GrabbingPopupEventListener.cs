using UnityEngine;

//Made by Amos Johan Persson

namespace MainGame.SpatialUI
{
    public class GrabbingPopupEventListener : MonoBehaviour
    {
        [SerializeField] PopUpController popUp;
        [SerializeField] Spirit.PossessibleItem urn;

        void Start()
        {
            var pusherScript = popUp.GetPlayer().GetComponent<WorldInterraction.ItemPusher>();

            if (pusherScript != null)
            {
                pusherScript.OnGrabbedItem += popUp.Disable;
                pusherScript.OnGrabbedItem += () => popUp.SetAllAlphas(0);
                pusherScript.OnReleasedItem += popUp.Enable;
            }

            if (urn == null || popUp == null)
                return;

            urn.OnPossessed += popUp.Enable;
            urn.OnUnPossessed += popUp.Disable;

            popUp.StartFadeAway();
            popUp.Disable();

            Managers.GameManager gameManager = GameObject.FindWithTag("GameManager")?.GetComponent<Managers.GameManager>();
            if (gameManager == null)
                return;

            gameManager.OnStartCinematics += popUp.Disable;
            gameManager.OnStartCinematics += () => popUp.SetAllAlphas(0);
            gameManager.OnFinishCinematics += popUp.Enable;
        }
    }
}
