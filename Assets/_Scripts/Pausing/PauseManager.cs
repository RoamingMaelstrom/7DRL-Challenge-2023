using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class PauseManager : MonoBehaviour
{
    [SerializeField] BoolSOEvent updatePauseCounterEvent;
    [SerializeField] SOEvent enterPauseEvent;
    [SerializeField] SOEvent exitPauseEvent;

    public int pauseCounter = 0;
    public bool isPaused = false;

    Coroutine gradualUnpauseRoutine;
    [SerializeField] [Range(0.01f, 1f)] float unpauseSpeedUpRate = 0.15f;

    private void Awake() 
    {
        updatePauseCounterEvent.AddListener(UpdatePauseCounter);  
        pauseCounter = 0;
        isPaused = false;
        Time.timeScale = 1;
    }

    private void UpdatePauseCounter(bool pauseRequest)
    {
        if (pauseRequest) pauseCounter ++;
        else pauseCounter --;
        UpdatePauseState();
    }

    private void UpdatePauseState()
    {
        if (pauseCounter > 0 && !isPaused)
        {
            enterPauseEvent.Invoke();
            isPaused = true;
            if (gradualUnpauseRoutine != null) StopCoroutine(gradualUnpauseRoutine);
            Time.timeScale = 0;
            return;
        }
        if (pauseCounter == 0 && isPaused) 
        {
            isPaused = false;
            gradualUnpauseRoutine = StartCoroutine(GradualUnpause());
            exitPauseEvent.Invoke();
            return;
        }
    }

    IEnumerator GradualUnpause()
    {
        while (Time.timeScale < 1)
        {
            Time.timeScale += unpauseSpeedUpRate;
            Time.timeScale = Mathf.Min(1f, Time.timeScale);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return null;
    }
}

