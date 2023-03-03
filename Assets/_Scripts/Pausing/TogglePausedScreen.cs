using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class TogglePausedScreen : MonoBehaviour
{
    [SerializeField] BoolSOEvent togglePauseScreenEvent;
    [SerializeField] GameObject pauseScreenContent;

    private void Awake() 
    {
        togglePauseScreenEvent.AddListener(ToggleContent);
    }

    private void ToggleContent(bool screenState)
    {
        if (screenState)
        {
            pauseScreenContent.SetActive(true);
            return;
        }
        pauseScreenContent.SetActive(false);
    }
}
