using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class TogglePause : MonoBehaviour
{
    [SerializeField] BoolSOEvent updatePauseCounterEvent;
    public bool statePaused = false;


    public void Toggle()
    {
        statePaused = !statePaused;
        updatePauseCounterEvent.Invoke(statePaused);
    }

}
