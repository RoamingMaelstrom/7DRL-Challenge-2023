using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class PauseScreenLogic : MonoBehaviour
{
    [SerializeField] BoolSOEvent pauseEvent;
    [SerializeField] GameObject pauseScreen;

    bool isPausing = false;
    public void TogglePause()
    {
        if (isPausing) 
        {
            pauseScreen.SetActive(false);
            pauseEvent.Invoke(false);
        }
        else 
        {
            pauseEvent.Invoke(true);
            pauseScreen.SetActive(true);
        }

        isPausing = !isPausing;
    }
}
