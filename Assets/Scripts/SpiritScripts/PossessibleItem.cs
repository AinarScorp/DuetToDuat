using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made by Einar Hallik

namespace MainGame.Spirit
{
    public class PossessibleItem : MonoBehaviour,IPossessable
    {
        [SerializeField] float maxBrightIntensity = 5, minBrightIntensity = 0;
        [SerializeField] float possessionIntensity = 10;

        [SerializeField] Transform shoulder;
        [SerializeField] float blinkingSpeed = 2;

        Material material;
        Coroutine visualisation;
        Coroutine changingIntensity;
        
        public event Action OnPossessed;
        public event Action OnUnPossessed;

        void Awake()
        {
            material = GetComponentInChildren<MeshRenderer>().material;
        }
        void Start()
        {
            StartCoroutine(ChangeIntensity(minBrightIntensity));
        }

        public void DestroyThis()
        {
            StartCoroutine(DestroyingThisScript());
        }
        IEnumerator DestroyingThisScript()
        {
            if (visualisation !=null )
            {
                StopCoroutine(visualisation);
            }
            if (changingIntensity!=null)
            {
                StopCoroutine(changingIntensity);
            }
            yield return ChangeIntensity(minBrightIntensity);
            Destroy(this);
        }
        
        public Transform GetShoulder()
        {
            return shoulder;
        }
        public void VisualisePossession()
        {
            if (visualisation!= null)
            {
                return;
            }
            visualisation = StartCoroutine(StartVisualizingPossession());
        }

        public void Possess()
        {
            if (visualisation !=null ||changingIntensity!=null)
            {
                StopAllCoroutines();
                visualisation = null;
            }
            changingIntensity = StartCoroutine(ChangeIntensity(possessionIntensity));
            OnPossessed?.Invoke();
        }

        public void UnPossess()
        {
            if (changingIntensity!=null)
            {
                StopCoroutine(changingIntensity);
            }
            changingIntensity = StartCoroutine(ChangeIntensity(minBrightIntensity));
            OnUnPossessed?.Invoke();
        }
        IEnumerator StartVisualizingPossession()
        {
            yield return ChangeIntensity(maxBrightIntensity);
            yield return ChangeIntensity(minBrightIntensity);
            visualisation = null;
        }
        IEnumerator ChangeIntensity(float intensityToReach)
        {
            if (material == null)
            {
                Debug.LogError("Material has not been found");
                yield break;
            }
            
            float brightnessIntensity = material.GetFloat("_BrightnessIntensity");;
            float percent = 0;
            while (percent <1)
            {
                percent += Time.deltaTime * blinkingSpeed;
                brightnessIntensity = Mathf.Lerp(brightnessIntensity, intensityToReach, percent);
                
                material.SetFloat("_BrightnessIntensity", brightnessIntensity);
                yield return new WaitForEndOfFrame();
            }
            changingIntensity = null;
        }
    }
    
}
