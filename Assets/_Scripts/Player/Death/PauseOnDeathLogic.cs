using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class PauseOnDeathLogic : MonoBehaviour
{
    [SerializeField] GameObjectFloatSOEvent playerDeathEvent;
    [SerializeField] BoolSOEvent pauseEvent;

    private void Awake() 
    {
        playerDeathEvent.AddListener(PauseOnDeath);
    }

    void PauseOnDeath(GameObject playerParent, float arg1)
    {
        pauseEvent.Invoke(true);
    }
}




