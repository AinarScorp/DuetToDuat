using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Max Ekberg.

//TODO Delete this.

public class TimerBase : MonoBehaviour
{

    [SerializeField] private float timerLengthInSeconds = 10f;

    private float timerDisplayValue;    
    
    protected Coroutine timerCoroutine; // Call in StartCoroutine to start a timer.

    protected bool timerDone = false;

    public float TimerDisplayValue { get => timerDisplayValue; }
    
    protected IEnumerator Timer(float currentTimerLength)
    {
        Debug.Log("Timer Starter");
        
        while (currentTimerLength > 0)
        {
            yield return new WaitForEndOfFrame();
            currentTimerLength -= Time.deltaTime;
            timerDisplayValue = currentTimerLength;
        }
        timerDone = true;
    }

    protected virtual void OnTimerDone()
    {
        timerDone = false;
        timerDisplayValue = timerLengthInSeconds;
        Debug.Log("Timer done");
    }    
}
