using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintFlashingLogic : MonoBehaviour
{
    [SerializeField] HintsLogic hintLogicScript;
    [SerializeField] Animator animator;

    private void OnEnable() 
    {
        if (!hintLogicScript.outOfOriginalHints) animator.SetTrigger("StartFlashing");
    }
}
