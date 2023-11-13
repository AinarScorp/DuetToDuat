using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Made by Amos Johan Persson

namespace MainGame.SpatialUI
{
    public class PopUpController : MonoBehaviour
    {
        [SerializeField] bool hiddenByDefault;
        [SerializeField] TextMeshProUGUI message;
        [SerializeField] Image background;
        [SerializeField] Image button;
        [SerializeField] Image icon;

        float messageAlphaInitial;
        float backgroundAlphaInitial;
        float buttonAlphaInitial;
        float iconAlphaInitial;

        [SerializeField] AnimationCurve fadeOutCurve;
        [SerializeField] AnimationCurve fadeInCurve;

        [SerializeField] float fadeOutTime;
        [SerializeField] float fadeInTime;

        [SerializeField] GameObject playerObject;

        private float elapsedTime;
        private bool isDisabled;

        private void Start()
        {
            GetInitialAlphas();
            if (hiddenByDefault) 
                SetAllAlphas(0);
        }

        public void StartFadeAway()
        {
            elapsedTime = 0;
            StartCoroutine(FadeOutAlphas());
        }

        public void StartFadeIn()
        {
            elapsedTime = 0;
            StartCoroutine(FadeInAlphas());
        }

        public void Disable()
        {
            isDisabled = true;
        }

        public void Enable()
        {
            isDisabled = false;
        }

        public bool IsReactivationEnabled()
        {
            return !isDisabled;
        }

        public GameObject GetPlayer()
        {
            return playerObject;
        }

        private void GetInitialAlphas()
        {
            messageAlphaInitial = message.faceColor.a;
            backgroundAlphaInitial = background.color.a;
            buttonAlphaInitial = button.color.a;
            iconAlphaInitial = icon.color.a;
        }

        IEnumerator FadeOutAlphas()
        {
            while (elapsedTime < fadeOutTime)
            {
                elapsedTime += Time.deltaTime;
                float interpolation = 1 - fadeOutCurve.Evaluate(elapsedTime / fadeOutTime);

                SetImageAlpha(background, interpolation * backgroundAlphaInitial);
                SetImageAlpha(button, interpolation * buttonAlphaInitial);
                SetImageAlpha(icon, interpolation * iconAlphaInitial);
                SetFontMaterialAlpha(message, interpolation * messageAlphaInitial);
                yield return null;
            }
        }

        IEnumerator FadeInAlphas()
        {

            while (elapsedTime < fadeInTime)
            {
                elapsedTime += Time.deltaTime;
                float interpolation = fadeInCurve.Evaluate(elapsedTime / fadeInTime);

                SetImageAlpha(background, interpolation * backgroundAlphaInitial);
                SetImageAlpha(button, interpolation * buttonAlphaInitial);
                SetImageAlpha(icon, interpolation * iconAlphaInitial);
                SetFontMaterialAlpha(message, interpolation * messageAlphaInitial);
                yield return null;
            }
        }

        public void SetAllAlphas(float value)
        {
            SetImageAlpha(background, value);
            SetImageAlpha(button, value);
            SetImageAlpha(icon, value);
            SetFontMaterialAlpha(message, value);
        }

        private void SetImageAlpha(Image im, float value)
        {
            Color32 color = new Color(im.color.r, im.color.g, im.color.b, value);
            im.color = color;
        }

        private void SetFontMaterialAlpha(TextMeshProUGUI text, float value)
        {
            Color oldColor = text.faceColor;
            Color32 color = new Color(oldColor.r, oldColor.g, oldColor.b, value);
            text.faceColor = color;
        }
    }
}
